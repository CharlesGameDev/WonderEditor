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
        return [self.x, self.y, self.z]

    def from_string(string):
        vector = Vector()
        split = string.split(",")
        vector.x = float(split[0])
        vector.y = float(split[1])
        vector.z = float(split[2])

        return vector
    
    def __str__(self) -> str:
        return f"{self.x, self.y, self.z}"
