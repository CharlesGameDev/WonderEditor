from actor import Actor
from info import *
import random
import copy
import yaml
from vector import Vector

class Level:
    def __init__(self, yaml) -> None:
        self.yaml = yaml

    def get_walls(self):
        if "BgUnits" in self.yaml["root"]:
            if "Walls" in self.yaml["root"]["BgUnits"][0]:
                return self.yaml["root"]["BgUnits"][0]["Walls"]
        return None

    def set_walls(self, walls):
        self.yaml["root"]["BgUnits"][0]["Walls"] = walls
    
    def get_actors(self, viewport = None, group=True):
        actors = []
        for a in self.yaml["root"]["Actors"]:
            actor = Actor(a)
            if group and viewport != None:
                if actor.type == None and not viewport.show_actor_other: continue
                if actor.type == ACTOR_ITEM and not viewport.show_actor_item: continue
                if actor.type == ACTOR_ENEMY and not viewport.show_actor_enemy: continue
                if actor.type == ACTOR_OBJECT and not viewport.show_actor_object: continue
                if actor.type == ACTOR_BLOCK and not viewport.show_actor_block: continue
                if actor.type == ACTOR_WORLD and not viewport.show_actor_world: continue
                if actor.type == ACTOR_AREA and not viewport.show_actor_area: continue
                if actor.type == ACTOR_MAP and not viewport.show_actor_map: continue
            actors.append(Actor(a))

        return actors

    def set_actor(self, hash, yaml):
        for i in range(len(self.yaml["root"]["Actors"])):
            actor = self.yaml["root"]["Actors"][i]
            if actor["Hash"] == hash:
                self.yaml["root"]["Actors"][i] = yaml

    def add_actor(self, gyaml: str, name: str, position: Vector):
        actor_yaml = {
            "AreaHash": self.get_actors()[0].get_area_hash(),
            "Gyaml": gyaml,
            "Hash": random.randrange(0, 99999999999999999999),
            "Layer": "PlayArea1",
            "Name": name,
            "Rotate": [0, 0, 0],
            "Scale": [1, 1, 1],
            "Translate": position.to_array(),
        }
        actor = Actor(actor_yaml)
        self.yaml["root"]["Actors"].append(actor.to_yaml())
        return actor

    def get_actor(self, hash):
        for a in self.yaml["root"]["Actors"]:
            if a["Hash"] == hash:
                return Actor(a)
            
    def remove_actor(self, hash):
        for a in self.yaml["root"]["Actors"]:
            if a["Hash"] == hash:
                self.yaml["root"]["Actors"].remove(a)

    def duplicate_actor(self, hash):
        for a in self.yaml["root"]["Actors"]:
            if a["Hash"] == hash:
                a2 = copy.copy(a)
                a2["Hash"] = random.randrange(0, 99999999999999999999)
                a2["Name"] += "(2)"
                translate = Vector(a2["Translate"])
                translate.x += 2
                translate.y += 2
                a2["Translate"] = translate.to_array()
                self.yaml["root"]["Actors"].append(a2)
                return a2["Hash"]

    def save(self, path):
        with open(path, "w") as file:
            yaml.dump(self.yaml, file, yaml.Dumper)
