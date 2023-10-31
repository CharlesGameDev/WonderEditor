import typing
from PyQt5 import QtGui
from PyQt5.QtWidgets import *
from PyQt5.QtCore import *
from PyQt5.QtGui import *
from level import Level
from vector import VectorInt
from tools import *
from info import *

class Viewport(QWidget):
    def __init__(self, parent):
        self.parent = parent
        super().__init__(self.parent)
        self.level: Level = None
        self.xoffset = 0
        self.yoffset = 200
        self.scale = 3
        self.qp = QPainter()
        self.walls = []
        self.actors = []
        self.hovering_actors = {}
        self.show_walls = True
        self.show_actors = True
        self.show_actor_names = True
        self.show_actor_gimmick = True
        self.show_actor_other = True
        self.mx = 0
        self.my = 0

    def set_level(self, level):
        self.level = level
        self.makeHoveringActors()

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
            if x >= rect.x() and y <= rect.y() and x <= rect.x() + rect.width() and y >= rect.y() + rect.height():
                self.hovering_actors[actor.hash] = True
            else:
                self.hovering_actors[actor.hash] = False

        self.mx = x
        self.my = y
        self.update()

    def mousePressEvent(self, event: QMouseEvent | None) -> None:
        if event.button() == Qt.LeftButton:
            for hash in self.hovering_actors:
                if self.hovering_actors[hash]:
                    self.parent.actor_selection.select(self, hash)
            self.parent.actor_selection.clearFocus()

    def makeHoveringActors(self):
        self.hovering_actors = {}
        actors = self.level.get_actors(self, False)
        for actor in actors:
            self.hovering_actors[actor.hash] = False

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

    def drawCurrentPoints(self):
        self.qp.setPen(GROUND_COLOR)

        if self.show_walls:
            for wall in self.walls:
                last_point = None
                for point in wall:
                    if last_point != None:
                        self.qp.drawLine((point.x + self.xoffset) * self.scale, (point.y + self.yoffset) * self.scale, (last_point.x + self.xoffset) * self.scale, (last_point.y + self.yoffset) * self.scale)
                    last_point = point

        hover_text = ""
        if self.show_actors:
            for actor in self.actors:
                self.qp.setPen(actor.get_color())
                rect = actor.make_rect(self)
                self.qp.drawRect(rect)
                if self.show_actor_names and self.hovering_actors[actor.hash]:
                    hover_text += actor.gyaml + "\n"

            if self.show_actor_names:
                self.qp.setPen(HOVER_TEXT_COLOR)
                self.qp.drawText(self.mx + 15, self.my, 500, 500, Qt.TextWordWrap, hover_text)
