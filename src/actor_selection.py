from PyQt5.QtGui import *
from PyQt5.QtWidgets import *
from PyQt5.QtCore import *
from actor import Actor

class ActorSelection(QToolBar):
    def __init__(self, parent, viewport) -> None:
        super().__init__("Actor Selection", parent)
        self.viewport = viewport
        self.setStyleSheet("background-color: black;")
        self.setStyleSheet("QToolBar::separator { background-color: white; height: 1px; margin-top: 15px; margin-bottom: 15px; }")
        self.setMinimumWidth(400)
        self.setMaximumWidth(self.size().width())
        self.setMovable(False)

        self.selected_actor: Actor = None

        self.visual_widget = QWidget()
        self.visual = QVBoxLayout(self)
        self.visual_widget.setLayout(self.visual)

        self.dynamic_widget = QWidget()
        self.dynamic = QVBoxLayout(self)
        self.dynamic_widget.setLayout(self.dynamic)

        self.nameLabel = self.make_input("", 14)
        self.gyamlLabel = self.make_input("No Actor Selected", 15)

        self.options_label = self.make_label("Options", 15)
        self.visual_button = self.make_button("Show Visual", 15, self.show_visual)
        self.dynamic_button = self.make_button("Show Dynamic", 15, self.show_dynamic)

        self.options_label.setHidden(True)
        self.visual_button.setHidden(True)
        self.dynamic_button.setHidden(True)

        self.hashLabel = self.make_combo("Hash:", "", 10, self.visual)[0]
        self.areaHashLabel = self.make_combo("Area Hash:", "", 10, self.visual)[0]
        self.layerLabel = self.make_combo("Layer:", "", 10, self.visual)[0]

        self.make_label("Translation", 12, self.visual)

        self.xInput = self.make_combo("X:", "0", 11, self.visual)[0]
        self.yInput = self.make_combo("Y:", "0", 11, self.visual)[0]
        self.zInput = self.make_combo("Z:", "0", 11, self.visual)[0]

        self.make_label("Scale", 12, self.visual)

        self.xsInput = self.make_combo("X:", "0", 11, self.visual)[0]
        self.ysInput = self.make_combo("Y:", "0", 11, self.visual)[0]
        self.zsInput = self.make_combo("Z:", "0", 11, self.visual)[0]

        self.make_label("Rotation", 12, self.visual)

        self.xrInput = self.make_combo("X:", "0", 11, self.visual)[0]
        self.yrInput = self.make_combo("Y:", "0", 11, self.visual)[0]
        self.zrInput = self.make_combo("Z:", "0", 11, self.visual)[0]

        self.addWidget(self.visual_widget)
        self.addWidget(self.dynamic_widget)

        self.make_button("Duplicate", 15, self.duplicate, self.visual)
        self.make_button("Apply Changes", 15, self.apply_changes, self.visual)
        self.make_button("Delete", 15, self.delete, self.visual)
        self.make_button("Apply Changes", 15, self.apply_changes, self.dynamic)

        self.hide_all()

        self.dynamic_values = {}

    def show_visual(self):
        self.set_children(self.visual_widget, True)
        self.set_children(self.dynamic_widget, False)

    def show_dynamic(self):
        self.set_children(self.visual_widget, False)
        self.set_children(self.dynamic_widget, True)

    def hide_all(self):
        self.set_children(self.visual_widget, False)
        self.set_children(self.dynamic_widget, False)
        self.visual_button.setHidden(True)
        self.dynamic_button.setHidden(True)
        self.options_label.setHidden(True)

    def set_children(self, parent, visible):
        for child in parent.children():
            try:
                child.setVisible(visible)
            except AttributeError:
                continue

    def remove_children(self, parent):
        for child in parent.children():
            try:
                child.remove()
            except AttributeError:
                continue

    def apply_changes(self):
        if self.selected_actor == None: return
        self.viewport.set_actor_data(self)
        self.viewport.update()

    def delete(self):
        if self.selected_actor == None: return
        self.viewport.delete_actor(self.selected_actor.get_hash())
        self.viewport.update()

    def duplicate(self):
        if self.selected_actor == None: return
        self.viewport.duplicate_actor(self.selected_actor.get_hash(), self)
        self.viewport.update()

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
    
    def set_default_values(self):
        self.selected_actor = None
        self.nameLabel.setText(self.nameLabel.accessibleName())
        self.gyamlLabel.setText(self.gyamlLabel.accessibleName())
        self.areaHashLabel.setText(self.areaHashLabel.accessibleName())
        self.hashLabel.setText(self.hashLabel.accessibleName())
        self.layerLabel.setText(self.layerLabel.accessibleName())
        self.xInput.setText(self.xInput.accessibleName())
        self.yInput.setText(self.yInput.accessibleName())
        self.zInput.setText(self.zInput.accessibleName())
        self.xsInput.setText(self.xsInput.accessibleName())
        self.ysInput.setText(self.ysInput.accessibleName())
        self.zsInput.setText(self.zsInput.accessibleName())
        self.xrInput.setText(self.xrInput.accessibleName())
        self.yrInput.setText(self.yrInput.accessibleName())
        self.zrInput.setText(self.zrInput.accessibleName())

        self.hide_all()

    def update(self, hash):
        self.selected_actor = self.viewport.level.get_actor(hash)
        if self.selected_actor == None:
            self.set_default_values()
            return

        self.nameLabel.setText(f"{self.selected_actor.get_name()}")
        self.gyamlLabel.setText(f"{self.selected_actor.get_gyaml()}")
        self.areaHashLabel.setText(f"{self.selected_actor.get_area_hash()}")
        self.hashLabel.setText(f"{self.selected_actor.get_hash()}")
        self.layerLabel.setText(f"{self.selected_actor.get_layer()}")
        translate = self.selected_actor.get_translate()
        scale = self.selected_actor.get_scale()
        rotate = self.selected_actor.get_rotate()
        self.xInput.setText(f"{translate.x}")
        self.yInput.setText(f"{translate.y}")
        self.zInput.setText(f"{translate.z}")
        self.xsInput.setText(f"{scale.x}")
        self.ysInput.setText(f"{scale.y}")
        self.zsInput.setText(f"{scale.z}")
        self.xrInput.setText(f"{rotate.x}")
        self.yrInput.setText(f"{rotate.y}")
        self.zrInput.setText(f"{rotate.z}")
        self.visual_button.setHidden(False)
        self.dynamic_button.setHidden(False)
        self.options_label.setHidden(True)

        for v in self.dynamic_values:
            self.dynamic.removeWidget(self.dynamic_values[v])
        self.dynamic_values = {}
        dynamic = self.selected_actor.get_dynamic()
        if dynamic != None:
            for d in dynamic:
                self.dynamic_values[d] = self.make_combo(d, str(dynamic[d]), 11, self.dynamic)[1]
        pass

    def select(self, hash):
        self.clearFocus()
        self.update(hash)

    def clearFocus(self) -> None:
        self.nameLabel.clearFocus()
        self.hashLabel.clearFocus()
        self.areaHashLabel.clearFocus()
        self.gyamlLabel.clearFocus()
        self.xInput.clearFocus()
        self.yInput.clearFocus()
        self.zInput.clearFocus()
        self.xsInput.clearFocus()
        self.ysInput.clearFocus()
        self.zsInput.clearFocus()
        self.xrInput.clearFocus()
        self.yrInput.clearFocus()
        self.zrInput.clearFocus()
        for v in self.dynamic_values:
            for child in self.dynamic_values[v].children():
                try:
                    child.clearFocus()
                except AttributeError:
                    continue
        return super().clearFocus()
