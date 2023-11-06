from PyQt5.QtGui import *
from PyQt5.QtWidgets import *
from PyQt5.QtCore import *
from info import *
from vector import Vector
from viewport import Viewport

class WallEditor(QMainWindow):
    def __init__(self, window, viewport: Viewport) -> None:
        super().__init__(None, Qt.WindowStaysOnTopHint)
        self.window = window
        self.viewport = viewport
        self.setStyleSheet("background-color: black; color: white;") 
        self.start_width = 800
        self.start_height = 600
        self.setGeometry(0, 0, self.start_width, self.start_height)
        self.setWindowTitle("Wall Editor")

        self.selectedGroup = -1
        self.selectedPoint = -1
        self.items = []
        self.points = []

        self.add_group_button = QPushButton(self)
        self.add_group_button.setText("New Group")
        self.add_group_button.setStyleSheet("background-color: white; color: black;")
        self.add_group_button.clicked.connect(self.new_group)

        self.add_point_button = QPushButton(self)
        self.add_point_button.setText("New Point")
        self.add_point_button.setStyleSheet("background-color: white; color: black;")
        self.add_point_button.clicked.connect(self.new_point)

        self.list = QListWidget(self)
        self.list.itemClicked.connect(self.item_clicked)
        self.list.itemSelectionChanged.connect(self.item_changed)

        self.point_list = QListWidget(self)
        self.point_list.itemClicked.connect(self.point_clicked)
        self.point_list.itemSelectionChanged.connect(self.point_changed)

        self.point_editor = QWidget(self)
        layout = QVBoxLayout(self.point_editor)

        self.indexLabel = self.make_label("", 11, layout)

        self.xInput = self.make_combo("X:", "0", 11, layout)[0]
        self.yInput = self.make_combo("Y:", "0", 11, layout)[0]
        self.zInput = self.make_combo("Z:", "0", 11, layout)[0]

        self.apply_button = self.make_button("Apply", 11, self.set_input_values, layout)
        self.remove_button = self.make_button("Delete", 11, self.remove_selected, layout)

        self.point_editor.setLayout(QVBoxLayout())

        self.no_stage_label = QLabel(self)
        self.no_stage_label.setAlignment(Qt.AlignCenter)
        font = self.no_stage_label.font()
        font.setPointSize(30)
        self.no_stage_label.setFont(font)
        self.no_stage_label.resize(self.size())
        self.no_stage_label.setText("No stage loaded.")
        self.no_stage_label.setStyleSheet("color: white;")

        self.resizeEvent()

        self.walls = []

    def item_clicked(self, item: QListWidgetItem):
        index = int(item.text())
        self.point_list.clear()
        self.points = []

        self.selectedGroup = index

        walls = self.walls[index]["ExternalRail"]["Points"]
        for i in range(len(walls)):
            wall = walls[i]
            vector = Vector(wall["Translate"])
            item = QListWidgetItem(str(vector))
            item.setToolTip(str(i))
            self.point_list.addItem(item)
            self.points.append(item)

    def item_changed(self):
        if len(self.list.selectedItems()) > 0:
            item = self.list.selectedItems()[-1]
            self.item_clicked(item)

    def point_clicked(self, item: QListWidgetItem):
        point = Vector.from_string(item.text().replace("(", "").replace(")", "").replace(" ", ""))
        self.selectedPoint = int(item.toolTip())

        self.xInput.setText(str(point.x))
        self.yInput.setText(str(point.y))
        self.zInput.setText(str(point.z))
        self.indexLabel.setText(f"Group {self.selectedGroup}\nPoint {self.selectedPoint}")

    def point_changed(self):
        if len(self.point_list.selectedItems()) > 0:
            item = self.point_list.selectedItems()[-1]
            self.point_clicked(item)

    def set_input_values(self):
        if self.selectedPoint < 0 or self.selectedGroup < 0: return

        vector = Vector()
        try:
            vector.x = float(self.xInput.text())
            vector.y = float(self.yInput.text())
            vector.z = float(self.zInput.text())
        except ValueError:
            return

        self.walls[self.selectedGroup]["ExternalRail"]["Points"][self.selectedPoint]["Translate"] = vector.to_array()
        self.viewport.level.set_walls(self.walls)
        self.update_points()
        self.viewport.update()
        self.item_clicked(self.items[self.selectedGroup])
        self.point_clicked(self.points[self.selectedPoint])

    def remove_selected(self):
        if self.selectedPoint < 0 or self.selectedGroup < 0: return

        self.walls[self.selectedGroup]["ExternalRail"]["Points"].pop(self.selectedPoint)
        self.viewport.level.set_walls(self.walls)
        self.update_points()
        self.viewport.update()
        self.item_clicked(self.items[self.selectedGroup])

    def new_group(self):
        group = {"ExternalRail": {"IsClosed": True, "Points": []}}
        self.walls.append(group)
        self.update_points()

    def new_point(self):
        if self.selectedGroup < 0: return

        point = {"Translate": Vector().to_array()}
        self.walls[self.selectedGroup]["ExternalRail"]["Points"].append(point)
        self.update_points()
        self.selectedPoint = len(self.points)
        self.item_clicked(self.items[self.selectedGroup])
        self.point_clicked(self.points[self.selectedPoint])

    def update_points(self):
        if self.viewport.level == None:
            self.no_stage_label.show()
            return

        self.no_stage_label.hide()
        self.walls = self.viewport.level.get_walls()
        self.list.clear()
        self.items = []
        for i in range(len(self.walls)):
            item = QListWidgetItem(str(i))
            self.list.addItem(item)
            self.items.append(item)

    def resizeEvent(self, event = None) -> None:
        self.no_stage_label.resize(self.size())

        listw = round(self.size().width() / 6)
        plistw = round(self.size().width() / 2)
        peditw = listw + plistw

        self.add_group_button.setGeometry(0, 0, listw, 25)
        self.add_point_button.setGeometry(listw, 0, plistw, 25)
        self.list.setGeometry(0, 25, listw, self.size().height() - 25)
        self.point_list.setGeometry(listw, 25, plistw, self.size().height() - 25)
        self.point_editor.setGeometry(peditw, 25, self.size().width() - peditw, self.size().height() - 25)

    def showEvent(self, event) -> None:
        self.update_points()

    def make_label(self, text, size, parent=None):
        if parent == None: parent = self
        label = QLabel(text, self)
        label.setAccessibleName(text)
        label.setStyleSheet("color: white; background-color: transparent;")
        label.setAlignment(Qt.AlignCenter)
        font = label.font()
        font.setPointSize(size)
        label.setFont(font)
        parent.addWidget(label)
        return label

    def make_input(self, text, size, parent=None):
        if parent == None: parent = self
        label = QLineEdit(text, self)
        label.setAccessibleName(text)
        label.setStyleSheet("color: white; background-color: transparent;")
        label.setAlignment(Qt.AlignCenter)
        font = label.font()
        font.setPointSize(size)
        label.setFont(font)
        parent.addWidget(label)
        return label

    def make_button(self, text, size, connect, parent=None):
        if parent == None: parent = self
        label = QPushButton(text, self)
        label.setAccessibleName(text)
        label.setStyleSheet("color: black; background-color: rgb(200, 200, 200);")
        font = label.font()
        font.setPointSize(size)
        label.setFont(font)
        label.clicked.connect(connect)
        parent.addWidget(label)
        return label
    
    def make_combo(self, title, default, size, parent=None):
        if parent == None: parent = self
        widget = QWidget()
        layout = QHBoxLayout()
        widget.setLayout(layout)

        self.make_label(title, size, layout)
        input = self.make_input(default, size, layout)

        parent.addWidget(widget)
        return input, widget
