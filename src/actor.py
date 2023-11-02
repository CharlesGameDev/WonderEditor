from PyQt5.QtGui import QColor, QImage
from PyQt5.QtCore import QRectF
from vector import Vector
from info import *

class Actor:
    def __init__(self, yaml) -> None:
        self.yaml = yaml

        self.type = None
        gyaml = self.get_gyaml()
        if gyaml.startswith("Item"): self.type = ACTOR_ITEM
        if gyaml.startswith("Enemy"): self.type = ACTOR_ENEMY
        if gyaml.startswith("Object"): self.type = ACTOR_OBJECT
        if gyaml.startswith("Block"): self.type = ACTOR_BLOCK
        if gyaml.startswith("World"): self.type = ACTOR_WORLD
        if gyaml.startswith("Area"): self.type = ACTOR_AREA
        if gyaml.startswith("Map"): self.type = ACTOR_MAP

    def get_dynamic(self):
        try:
            return self.yaml["Dynamic"]
        except KeyError:
            return None
        
    def set_dynamic(self, key, value):
        self.yaml["Dynamic"].update({key: value})

    def set_gyaml(self, new_gyaml):
        self.yaml["Gyaml"] = new_gyaml

    def get_gyaml(self) -> str:
        return self.yaml["Gyaml"]
    
    def get_hash(self):
        return self.yaml["Hash"]
    
    def get_area_hash(self):
        return self.yaml["AreaHash"]
    
    def set_layer(self, new_layer):
        self.yaml["Layer"] = new_layer

    def get_layer(self):
        return self.yaml["Layer"]
    
    def set_name(self, new_name):
        self.yaml["Name"] = new_name

    def get_name(self):
        return self.yaml["Name"]
    
    def get_rotate(self):
        return Vector(self.yaml["Rotate"])

    def get_scale(self):
        return Vector(self.yaml["Scale"])
    
    def get_translate(self):
        return Vector(self.yaml["Translate"])
    
    def set_rotate(self, rotate: Vector):
        self.yaml["Rotate"] = rotate.to_array()
    
    def set_scale(self, scale: Vector):
        self.yaml["Scale"] = scale.to_array()
    
    def set_translate(self, translate: Vector):
        self.yaml["Translate"] = translate.to_array()
    
    def make_rect(self, viewport) -> QRectF:
        return QRectF((self.get_translate().x + viewport.xoffset) * viewport.scale, (-self.get_translate().y + viewport.yoffset) * viewport.scale, self.get_scale().x * viewport.scale, self.get_scale().y * viewport.scale)
    
    def to_yaml(self):
        return self.yaml;
