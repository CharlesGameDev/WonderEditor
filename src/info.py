from PyQt5.QtCore import *
from PyQt5.QtGui import QColor, QImage

VERSION = "beta 1.0.0"
NAME = "Wonder Editor"
AUTHOR = "CharlesDev"

VIEWPORT_MOVE_RIGHT = [Qt.Key_D, Qt.Key_Right]
VIEWPORT_MOVE_LEFT = [Qt.Key_A, Qt.Key_Left]
VIEWPORT_MOVE_UP = [Qt.Key_W, Qt.Key_Up]
VIEWPORT_MOVE_DOWN = [Qt.Key_S, Qt.Key_Down]

HOVER_TEXT_COLOR = QColor(255, 0, 0)
GROUND_COLOR = QColor(0, 0, 255)
DEFAULT_ACTOR_COLOR = QColor(255, 255, 255)

ACTOR_ITEM = 0
ACTOR_ENEMY = 1
ACTOR_OBJECT = 2
ACTOR_BLOCK = 3
ACTOR_WORLD = 4
ACTOR_AREA = 5
ACTOR_MAP = 6

ACTOR_IMAGES = {
    "ObjectTalkingFlowerS": QImage("img/ObjectTalkingFlower.png"),
    "BlockRengaLight": QImage("img/BlockRenga.png"),
    "BlockRengaItem": QImage("img/BlockRenga.png"),
}

COMPILED_ACTOR_IMAGES = {}

def get_actor_image(gyaml: str):
    if gyaml in ACTOR_IMAGES:
        return ACTOR_IMAGES[gyaml]
    if gyaml not in COMPILED_ACTOR_IMAGES:
        img = QImage(f"img/{gyaml}.png")
        COMPILED_ACTOR_IMAGES[gyaml] = img
        return img
    return COMPILED_ACTOR_IMAGES[gyaml]

ACTOR_NAMES = {
    "ItemKinoko": "Super Mushroom",
    "ItemStar": "Super Star",
    "ItemOneUpKinoko": "1-Up Mushroom",
    "ItemWonderHole": "Wonder Flower",
    "ItemWonderChip": "Wonder Token",
    "ItemOffering": "Wonder Seed",
    "WorldMapDemoMotherWonderSeed": "Royal Seed",
    "ObjectCoinYellow": "Coin",
    "ObjectCoinRandom": "Flower Coin",
    "ObjectMiniLuckyCoin": "Mini Flower Coin",
    "ObjectBigTenLuckyCoin": "10-Flower Coin",
    "ItemBalloon": "Item Balloon",
    "ObjectGoalPole": "Goal Pole",
    "ObjectCoinYellow": "Coin",
    "BlockRenga": "Block",
    "BlockHatena": "? Block",
    "BlockClarity": "Invisible Block",
    "ObjectBlockHardBreakable": "Hard Block",
    "ObjectBlockEndurance": "Jewel Block",
    "ObjectDashFloor": "Zip Track",
    "ObjectBlockPole": "Pole Block",
    "ObjectBlockSurpriseYellow": "! Block",
    "ObjectBlockRaceStart": "Race Block",
    "ObjectSoundBlinkBlock": "Rhythm Block",
    "ObjectTimerSwitchBlockSync": "timer switch",
    "ObjectSinkBlock": "Puffy Lift",
    "ObjectAshibaDisappearStep": "Dropdown Countdown Lift",
    "ObjectBlockLift": "Linking Lift",
    "ObjectDokan": "pipe",
    "ObjectXylophoneBridge": "Marimba Block",
    "AreaGelField": "goo",
    "ObjectPropellerFlowerForCourse": "Propeller Flower",
    "ObjectBlockHotStone": "Hot-Hot Rock",
    "ObjectGoQKun": "Downpour Cloud",
    "EnemyKuribo": "Goomba",
    "ObjectTalkingFlower": "Talking Flower",
    "ObjectTalkingFlowerS": "Talking Flower (Sky)",
    "AreaWaterBox": "Water",
}
