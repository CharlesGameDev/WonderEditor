from actor import Actor
from info import ACTOR_GIMMICK

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
                if actor.type == ACTOR_GIMMICK and not viewport.show_actor_gimmick: continue
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
