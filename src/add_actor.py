from PyQt5.QtCore import QEvent, QObject
from PyQt5.QtGui import *
from PyQt5.QtWidgets import *
from PyQt5.QtCore import *
from info import *
from tools import resize_label

class AddActor(QMainWindow):
    def __init__(self, window) -> None:
        super().__init__()
        self.window = window
        self.setStyleSheet("background-color: black; color: white;") 
        self.start_width = 800
        self.start_height = 600
        self.setGeometry(0, 0, self.start_width, self.start_height)
        self.setWindowTitle("Add Actor")

        self.selected = None

        self.search_text = QLabel(self)
        self.search_text.setText("Search")
        self.search_text.setAlignment(Qt.AlignCenter)
        self.search_text.setGeometry(0, 0, 400, 25)

        self.search_bar = QTextEdit(self)
        self.search_bar.setAlignment(Qt.AlignCenter)
        self.search_bar.setGeometry(0, 25, 400, 25)
        self.search_bar.setStyleSheet("background-color: white; color: black;")
        self.search_bar.textChanged.connect(self.search_item)

        self.list_widget = QListWidget(self)
        self.list_widget.setGeometry(0, 50, 400, 550)

        self.list_widget.itemClicked.connect(self.item_clicked)
        self.list_widget.itemSelectionChanged.connect(self.item_changed)

        for actor in ACTOR_NAMES:
            item = QListWidgetItem(f"{ACTOR_NAMES[actor]}ㅤ({actor})")
            self.list_widget.addItem(item)

        self.right_label_big = QLabel(self)
        self.right_label_big.setGeometry(400, 10, 400, 50)
        self.right_label_big.setStyleSheet("background: none;")
        self.right_label_big.setText("None")
        self.right_label_big.setAlignment(Qt.AlignCenter)
        font = self.right_label_big.font()
        font.setPointSize(30)
        self.right_label_big.setFont(font)

        self.right_label_small = QLabel(self)
        self.right_label_small.setGeometry(400, 60, 400, 30)
        self.right_label_small.setStyleSheet("background: none;")
        self.right_label_small.setText("None")
        self.right_label_small.setStyleSheet("color: gray;")
        self.right_label_small.setAlignment(Qt.AlignCenter)
        font = self.right_label_small.font()
        font.setPointSize(15)
        self.right_label_small.setFont(font)

        self.preview_image = QLabel(self)
        self.preview_image.setAlignment(Qt.AlignCenter)
        self.preview_image.setMinimumSize(1, 1)
        self.preview_image.setGeometry(500, 100, 200, 400)
        self.preview_image.hide()

        self.add_button = QPushButton(self)
        self.add_button.setStyleSheet("background-color: white; color: black;")
        self.add_button.setGeometry(500, 500, 200, 50)
        self.add_button.setText("Add Actor")
        self.add_button.clicked.connect(self.add_actor)
        font = self.add_button.font()
        font.setPointSize(20)
        self.add_button.setFont(font)

    def resizeEvent(self, event) -> None:
        self.list_widget.setGeometry(0, 50, round(self.width() / 2), self.height() - 50)
        self.search_bar.setGeometry(0, 25, round(self.width() / 2), 25)
        self.search_text.setGeometry(0, 0, round(self.width() / 2), 25)
        self.add_button.setGeometry(round(self.width() / 1.6), self.height() - 100, 200, 50)
        self.right_label_big.setGeometry(round(self.width() / 2), 10, round(self.width() / 2), 50)
        self.right_label_small.setGeometry(round(self.width() / 2), 60, round(self.width() / 2), 30)
        self.preview_image.setGeometry(round(self.width() / 1.6), 100, 200, 400)

    def add_actor(self):
        if self.selected == None: return
        self.hide()

    def search_item(self):
        search_string = self.search_bar.toPlainText()
        match_items = self.list_widget.findItems(search_string, Qt.MatchContains)
        for i in range(self.list_widget.count()):
            it = self.list_widget.item(i)
            it.setHidden(it not in match_items)

    def item_changed(self):
        item = self.list_widget.selectedItems()[-1]
        self.item_clicked(item)

    def item_clicked(self, item):
        self.selected = item
        names = item.text().split("ㅤ")
        self.right_label_big.setText(names[0])
        resize_label(self.right_label_big)
        self.right_label_small.setText(names[1])
        resize_label(self.right_label_small)
        image = get_actor_image(names[1].replace("(", "").replace(")", ""))
        if image != None:
            self.preview_image.show()
            pixmap = QPixmap.fromImage(image)
            pixmap = pixmap.scaled(self.preview_image.width(), self.preview_image.height(), Qt.KeepAspectRatio)
            self.preview_image.setPixmap(pixmap)
        else:
            self.preview_image.hide()
