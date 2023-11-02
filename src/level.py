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
        return self.yaml["root"]["BgUnits"][0]["Walls"]

    def set_walls(self, walls):
        self.yaml["root"]["BgUnits"][0]["Walls"] = walls
    
    def get_actors(self, viewport, group=True):
        actors = []
        for a in self.yaml["root"]["Actors"]:
            actor = Actor(a)
            if group:
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
                a2["Hash"] = random.randrange(10000000000000000000, 99999999999999999999)
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
