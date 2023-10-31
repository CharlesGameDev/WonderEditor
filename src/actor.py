from PyQt5.QtGui import QColor
from PyQt5.QtCore import QRectF
from vector import Vector
from info import DEFAULT_ACTOR_COLOR, ACTOR_COLORS, ACTOR_GIMMICK

class Actor:
    def __init__(self, yaml) -> None:
        self.yaml = yaml
        self.area_hash = yaml["AreaHash"]
        self.gyaml = yaml["Gyaml"]
        self.hash = yaml["Hash"]
        self.layer = yaml["Layer"]
        self.name = yaml["Name"]

        self.type = None
        for actor in ACTOR_GIMMICK:
            if actor in self.gyaml:
                self.type = ACTOR_GIMMICK
                break

        self.rotate = Vector(yaml["Rotate"])
        self.scale = Vector(yaml["Scale"])
        self.translate = Vector(yaml["Translate"])

    def get_color(self) -> QColor:
        if self.gyaml in ACTOR_COLORS:
            return ACTOR_COLORS[self.gyaml]

        return DEFAULT_ACTOR_COLOR

    def make_rect(self, viewport) -> QRectF:
        return QRectF((self.translate.x + viewport.xoffset) * viewport.scale, (self.translate.y + viewport.yoffset) * viewport.scale, self.scale.x * viewport.scale, self.scale.y * viewport.scale)

    def to_yaml(self):
        return dict(
            AreaHash = self.area_hash,
            Gyaml = self.gyaml,
            Hash = self.hash,
            Layer = self.layer,
            Name = self.name,
            Rotate = self.rotate.to_array(),
            Scale = self.scale.to_array(),
            Translate = self.translate.to_array(),
        )
