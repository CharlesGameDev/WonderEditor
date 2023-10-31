from PyQt5.QtGui import *
from PyQt5.QtWidgets import *
from PyQt5.QtCore import *

class ActorSelection(QToolBar):
    def __init__(self, parent) -> None:
        super().__init__("Actor Selection", parent)
        self.setStyleSheet("background-color: black;")
        self.setStyleSheet("QToolBar::separator { background-color: white; height: 1px; }")
        self.setMinimumWidth(400)
        self.setMaximumWidth(self.size().width())
        self.setMovable(False)

        self.nameLabel = self.make_input("", 14)
        self.addWidget(self.nameLabel)

        self.gyamlLabel = self.make_input("No Actor Selected", 15)
        self.addWidget(self.gyamlLabel)

        self.addSeparator()

        self.translationLabel = self.make_label("Translation", 12)
        self.addWidget(self.translationLabel)

    def make_label(self, text, size):
        label = QLabel(text, self)
        label.setStyleSheet("color: white; background-color: transparent;")
        label.resize(self.width(), 50)
        label.setAlignment(Qt.AlignCenter)
        font = label.font()
        font.setPointSize(size)
        label.setFont(font)
        return label

    def make_input(self, text, size):
        label = QLineEdit(text, self)
        label.setStyleSheet("color: white; background-color: transparent;")
        label.resize(self.width(), 50)
        label.setAlignment(Qt.AlignCenter)
        font = label.font()
        font.setPointSize(size)
        label.setFont(font)
        return label

    def select(self, viewport, hash):
        actor = viewport.level.get_actor(hash)
        self.clearFocus()
        self.nameLabel.setText(f"{actor.name}")
        self.gyamlLabel.setText(f"{actor.gyaml}")

    def clearFocus(self) -> None:
        self.nameLabel.clearFocus()
        self.gyamlLabel.clearFocus()
        return super().clearFocus()
