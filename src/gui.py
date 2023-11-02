from PyQt5.QtGui import *
from PyQt5.QtWidgets import *
from PyQt5.QtCore import *
from tools import *
from viewport import Viewport
from level import Level
from actor_selection import ActorSelection
from add_actor import AddActor
import yaml
import info
import sys

class Window(QMainWindow):
    def __init__(self) -> None:
        app = QApplication(sys.argv)
        super().__init__()
        app.installEventFilter(self)
        self.setStyleSheet("background-color: black;") 
        self.setGeometry(0, 0, 1280, 720)
        self.setWindowTitle(info.NAME)

        self.no_stage_label = QLabel(self)
        self.no_stage_label.setAlignment(Qt.AlignCenter)
        font = self.no_stage_label.font()
        font.setPointSize(30)
        self.no_stage_label.setFont(font)
        self.no_stage_label.resize(self.size())
        self.no_stage_label.setText("No stage loaded.")
        self.no_stage_label.setStyleSheet("color: white;")

        self.viewport = Viewport(self)
        self.setCentralWidget(self.viewport)

        self.viewport.setFocus()

        self.statusBar = QStatusBar()
        self.statusBar.showMessage(f"{info.NAME} | {info.VERSION} | {info.AUTHOR}", 100000)
        self.statusBar.setStyleSheet("color: white;")
        self.setStatusBar(self.statusBar)

        self.setup_menubar()

        self.actor_selection = ActorSelection(self, self.viewport)
        self.addToolBar(Qt.RightToolBarArea, self.actor_selection)

        self.timer = QTimer(self)
        self.timer.setSingleShot(False)
        self.timer.setInterval(16) # ~60 FPS
        self.timer.timeout.connect(self.timer_loop)
        self.timer.start()

        self.key_right = False
        self.key_left = False
        self.key_up = False
        self.key_down = False

        self.add_actor_window = AddActor(self)

        self.showMaximized()
        sys.exit(app.exec_())

    def timer_loop(self):
        if self.key_right:
            self.viewport.xoffset -= 1
            self.viewport.update()
        if self.key_left:
            self.viewport.xoffset += 1
            self.viewport.update()
        if self.key_up:
            self.viewport.yoffset += 1
            self.viewport.update()
        if self.key_down:
            self.viewport.yoffset -= 1
            self.viewport.update()

    def keyPressEvent(self, event: QKeyEvent | None) -> None:
        key = event.key()
        if key in info.VIEWPORT_MOVE_RIGHT: self.key_right = True
        if key in info.VIEWPORT_MOVE_LEFT: self.key_left = True
        if key in info.VIEWPORT_MOVE_UP: self.key_up = True
        if key in info.VIEWPORT_MOVE_DOWN: self.key_down = True
    
    def keyReleaseEvent(self, event: QKeyEvent | None) -> None:
        key = event.key()
        if key in info.VIEWPORT_MOVE_RIGHT: self.key_right = False
        if key in info.VIEWPORT_MOVE_LEFT: self.key_left = False
        if key in info.VIEWPORT_MOVE_UP: self.key_up = False
        if key in info.VIEWPORT_MOVE_DOWN: self.key_down = False

    def wheelEvent(self, event: QWheelEvent | None) -> None:
        if event.angleDelta().y() > 0:
            self.viewport.scale += 1
            self.viewport.update()
        else:
            self.viewport.scale -= 1
            self.viewport.update()

    def resizeEvent(self, event) -> None:
        self.no_stage_label.resize(self.size())
        self.viewport.resize(self.size())

    def setup_menubar(self):
        menubar = QMenuBar(self)
        menubar.setStyleSheet("background-color: gray;")

        fileMenu = QMenu("File", self)
        fileMenu.setStyleSheet("background-color: gray;")
        fileMenu.addAction(make_action("Open", self, self.open, "Ctrl+O"))
        fileMenu.addAction(make_action("Save", self, self.save, "Ctrl+S"))
        fileMenu.addAction(make_action("Save As...", self, self.save_as, "Ctrl+Shift+S"))
        fileMenu.addSeparator()
        fileMenu.addAction(make_action("Exit", self, sys.exit))

        editMenu = QMenu("Edit", self)
        editMenu.setStyleSheet("background-color: gray;")
        editMenu.addAction(make_action("Undo", self, self.undo, "Ctrl+Z"))
        editMenu.addAction(make_action("Redo", self, self.redo, "Ctrl+Y"))
        editMenu.addSeparator()
        editMenu.addAction(make_action("Cut", self, self.cut, "Ctrl+X"))
        editMenu.addAction(make_action("Copy", self, self.copy, "Ctrl+C"))
        editMenu.addAction(make_action("Paste", self, self.paste, "Ctrl+V"))
        editMenu.addSeparator()
        editMenu.addAction(make_action("Find", self, self.find, "Ctrl+F"))
        editMenu.addAction(make_action("Replace", self, self.replace, "Ctrl+H"))

        viewmenu = QMenu("View", self)
        viewmenu.setStyleSheet("background-color: gray;")
        viewmenu.addAction(make_action("Actor Selection", self, self.show_actor_selection, "Alt+S", True, True))
        viewmenu.addSeparator()
        viewmenu.addAction(make_action("Walls", self, self.show_walls, "Alt+W", True, True))
        viewmenu.addAction(make_action("Actors", self, self.show_actors, "Alt+A", True, True))
        viewmenu.addAction(make_action("Actor Names", self, self.show_actor_names, "Alt+N", True, True))
        viewmenu.addSeparator()
        viewmenu.addAction(make_action("Item Actors", self, self.show_actor_item, "Alt+I", True, True))
        viewmenu.addAction(make_action("Enemy Actors", self, self.show_actor_enemy, "Alt+E", True, True))
        viewmenu.addAction(make_action("Object Actors", self, self.show_actor_object, "Alt+O", True, True))
        viewmenu.addAction(make_action("Block Actors", self, self.show_actor_block, "Alt+B", True, True))
        viewmenu.addAction(make_action("World Actors", self, self.show_actor_world, "Alt+V", True, True))
        viewmenu.addAction(make_action("Map Actors", self, self.show_actor_map, "Alt+M", True, True))
        viewmenu.addAction(make_action("Other Actors", self, self.show_actor_other, "Alt+U", True, True))
        
        addmenu = QMenu("Add", self)
        addmenu.setStyleSheet("background-color: gray;")
        addmenu.addAction(make_action("Actor", self, self.add_actor, "Ctrl+Alt+A"))

        menubar.addMenu(fileMenu)
        menubar.addMenu(editMenu)
        menubar.addMenu(viewmenu)
        menubar.addMenu(addmenu)

        self.setMenuBar(menubar)

    def eventFilter(self, source, event):
        if event.type() == QEvent.MouseMove:
            pos = event.pos()
            self.viewport.mouseMove(pos.x(), pos.y())
        return QMainWindow.eventFilter(self, source, event)
    
    def add_actor(self):
        self.add_actor_window.show()

    def open(self):
        fileName, _ = QFileDialog.getOpenFileName(self, "Open Map File (*.yaml)", "", "Map Files (*.yaml);;All Files (*)")
        if not fileName:
            self.statusBar.showMessage("Open aborted, no file given", 2000)
            return

        self.no_stage_label.hide()

        if not fileName.endswith(".yaml"):
            return

        with open(fileName, 'r') as file:
            data = yaml.load(file, Loader=yaml.Loader)

            self.viewport.set_level(Level(data))

            self.statusBar.showMessage(f"Loaded '{fileName}'", 2000)

    def save(self):
        pass

    def save_as(self):
        fileName, _ = QFileDialog.getSaveFileName(self,"Save Map File (*.yaml)","","Map Files (*.yaml);;All Files (*)")
        if fileName:
            self.viewport.save_level(fileName)

    def undo(self):
        pass

    def redo(self):
        pass

    def cut(self):
        pass

    def copy(self):
        pass

    def paste(self):
        pass

    def find(self):
        pass

    def replace(self):
        pass

    def show_actor_selection(self, show):
        if show:
            self.actor_selection.show()
        else:
            self.actor_selection.hide()

    def show_walls(self, show: bool):
        self.viewport.show_walls = show
        self.viewport.update()

    def show_actors(self, show: bool):
        self.viewport.show_actors = show
        self.viewport.update()

    def show_actor_names(self, show: bool):
        self.viewport.show_actor_names = show
        self.viewport.update()

    def show_actor_item(self, show: bool):
        self.viewport.show_actor_item = show
        self.viewport.update()

    def show_actor_enemy(self, show: bool):
        self.viewport.show_actor_enemy = show
        self.viewport.update()

    def show_actor_object(self, show: bool):
        self.viewport.show_actor_object = show
        self.viewport.update()

    def show_actor_block(self, show: bool):
        self.viewport.show_actor_block = show
        self.viewport.update()

    def show_actor_world(self, show: bool):
        self.viewport.show_actor_world = show
        self.viewport.update()

    def show_actor_area(self, show: bool):
        self.viewport.show_actor_area = show
        self.viewport.update()

    def show_actor_map(self, show: bool):
        self.viewport.show_actor_map = show
        self.viewport.update()

    def show_actor_other(self, show: bool):
        self.viewport.show_actor_other = show
        self.viewport.update()

if __name__ == '__main__':
    Window()