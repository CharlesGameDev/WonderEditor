from PyQt5.QtWidgets import *
from PyQt5.QtCore import *
from PyQt5.QtGui import *
from level import Level
from vector import *
from tools import *
import math
from info import *
from actor_selection import ActorSelection

class Viewport(QWidget):
    def __init__(self, parent):
        self.parent = parent
        super().__init__(self.parent)
        self.level: Level = None
        self.xoffset = 0
        self.yoffset = 75
        self.scale = 6
        self.qp = QPainter()
        self.walls = []
        self.actors = []
        self.hovering_actors = {}
        self.show_walls = True
        self.show_actors = True
        self.show_actor_names = True
        self.show_actor_item = True
        self.show_actor_enemy = True
        self.show_actor_object = True
        self.show_actor_area = True
        self.show_actor_world = True
        self.show_actor_block = True
        self.show_actor_map = True
        self.show_actor_other = True
        self.mx = 0
        self.my = 0

    def set_level(self, level):
        self.level = level
        self.makeHoveringActors()

    def delete_actor(self, hash):
        self.level.remove_actor(hash)

    def duplicate_actor(self, hash, actor_selection):
        actor = self.level.duplicate_actor(hash)
        actor_selection.select(actor)

    def set_actor_data(self, actor_selection: ActorSelection):
        x = float(actor_selection.xInput.text())
        y = float(actor_selection.yInput.text())
        z = float(actor_selection.zInput.text())
        xs = float(actor_selection.xsInput.text())
        ys = float(actor_selection.ysInput.text())
        zs = float(actor_selection.zsInput.text())
        xr = float(actor_selection.xrInput.text())
        yr = float(actor_selection.yrInput.text())
        zr = float(actor_selection.zrInput.text())
        name = actor_selection.nameLabel.text()
        gyaml = actor_selection.gyamlLabel.text()

        translate = actor_selection.selected_actor.get_translate()
        translate.x = x
        translate.y = y
        translate.z = z
        scale = actor_selection.selected_actor.get_scale()
        scale.x = xs
        scale.y = ys
        scale.z = zs
        rotate = actor_selection.selected_actor.get_rotate()
        rotate.x = xr
        rotate.y = yr
        rotate.z = zr
        actor_selection.selected_actor.set_translate(translate)
        actor_selection.selected_actor.set_scale(scale)
        actor_selection.selected_actor.set_rotate(rotate)
        actor_selection.selected_actor.set_name(name)
        actor_selection.selected_actor.set_gyaml(gyaml)

        for dynamic in actor_selection.dynamic_values:
            layout = actor_selection.dynamic_values[dynamic]
            lineEdit = layout.findChild(QLineEdit)
            label = layout.findChild(QLabel)
            actor_selection.selected_actor.set_dynamic(label.text(), lineEdit.text())
            print(label.text(), actor_selection.selected_actor.get_dynamic()[label.text()])

        self.level.set_actor(actor_selection.selected_actor.get_hash(), actor_selection.selected_actor.to_yaml())

    def paintEvent(self, event: QPaintEvent | None = None) -> None:
        if self.level == None: return

        self.makeCurrentPoints()

        if self.scale < 1: self.scale = 1
        
        self.qp.begin(self)
        self.drawCurrentPoints()
        self.qp.end()

    def mouseMove(self, x, y) -> None:
        for actor in self.actors:
            rect = actor.make_rect(self)
            if x > rect.x() and y > rect.y() and x < rect.x() + rect.width() and y < rect.y() + rect.height():
                self.hovering_actors[actor.get_hash()] = True
            else:
                self.hovering_actors[actor.get_hash()] = False
        self.mx = x
        self.my = y
        self.update()

    def mousePressEvent(self, event: QMouseEvent | None) -> None:
        if event.button() == Qt.LeftButton:
            for hash in self.hovering_actors:
                if self.hovering_actors[hash]:
                    self.parent.actor_selection.select(hash)
            self.parent.actor_selection.clearFocus()

    def makeHoveringActors(self):
        self.hovering_actors = {}
        actors = self.level.get_actors(self, False)
        for actor in actors:
            self.hovering_actors[actor.get_hash()] = False

    def makeCurrentPoints(self):
        self.walls = []
        self.actors = []

        walls = self.level.get_walls()
        for external_rail in walls:
            wall = []
            for point in external_rail["ExternalRail"]["Points"]:
                point = VectorInt(point["Translate"])
                wall.append(point)
            self.walls.append(wall)

        self.actors = self.level.get_actors(self)

    def save_level(self, path):
        self.level.save(path)

    def drawCurrentPoints(self):
        self.qp.setPen(GROUND_COLOR)

        if self.show_walls:
            for wall in self.walls:
                last_point = None
                for point in wall:
                    if last_point != None:
                        self.qp.drawLine((point.x + self.xoffset) * self.scale, (-point.y + self.yoffset) * self.scale, (last_point.x + self.xoffset) * self.scale, (-last_point.y + self.yoffset) * self.scale)
                    last_point = point

        hover_text = ""
        if self.show_actors:
            for actor in self.actors:
                self.qp.setPen(DEFAULT_ACTOR_COLOR)
                rect = actor.make_rect(self)
                img = get_actor_image(actor.get_gyaml())
                if img:
                    self.qp.drawImage(rect, img)
                self.qp.drawRect(rect)
                if actor.get_hash() in self.hovering_actors:
                    if self.show_actor_names and self.hovering_actors[actor.get_hash()]:
                        hover_text += actor.get_gyaml() + "\n"

            if self.show_actor_names:
                self.qp.setPen(HOVER_TEXT_COLOR)
                self.qp.drawText(self.mx + 15, self.my, 500, 500, Qt.TextWordWrap, hover_text)
