class Vector:
    def __init__(self, translate = None) -> None:
        if translate == None:
            self.x = 0
            self.y = 0
            self.z = 0
            return

        self.x: float = float(translate[0])
        self.y: float = float(translate[1])
        self.z: float = float(translate[2])

    def to_array(self):
        return self.x, self.y, self.z

class VectorInt:
    def __init__(self, translate) -> None:
        self.x: int = round(float(translate[0]))
        self.y: int = round(float(translate[1]))
        self.z: int = round(float(translate[2]))

    def to_array(self):
        return self.x, self.y, self.z
