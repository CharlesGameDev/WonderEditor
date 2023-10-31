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

ACTOR_COLORS = {
    "EnemyKuribo": QColor(165, 42, 42), # goomba
    "ObjectTalkingFlower": QColor(255, 165, 0), # talking flower
    "ObjectTalkingFlowerS": QColor(255, 165, 0), # talking flower S
    "ObjectDokan": QColor(0, 255, 0), # pipe
    "ObjectCoinYellow": QColor(255, 255, 0), # gold coin
    "ObjectCoinRandom": QColor(203, 195, 227), # flower coin
    "ObjectBigTenLuckyCoin": QColor(128,0,128), # 10-flower coin
    "BlockRenga": QColor(92, 64, 51), # brick block
    "BlockRengaItem": QColor(112, 84, 71), # brick block w/ item
    "BlockRengaLight": QColor(192, 164, 151), # breakable brick block
    "BlockHatena": QColor(255, 255, 0), # ? block
    "ObjectGoalPole": QColor(0, 128, 0), # goal pole
}

# gimmick actor list
ACTOR_GIMMICK = [
    "ItemKinoko",
    "EnemyKuribo",
    "ObjectTalkingFlower",
    "ObjectTalkingFlowerS",
    "ObjectGoalPole",
    "ItemStar",
    "ItemOneUpKinoko",
    "ItemWonderHole",
    "ItemWonderChip",
    "ItemOffering",
    "WorldMapDemoMotherWonderSeed",
    "ObjectCoinYellow",
    "ObjectCoinRandom",
    "ObjectBigTenLuckyCoin",
    "ObjectTreasureChest",
    "ItemBalloon",
    "ObjectGoalPole",
    "BlockRenga",
    "BlockHatena",
    "ObjectBlockHardBreakable",
    "ObjectBlockEndurance",
    "ObjectDashFloor",
    "ObjectBlockPole",
    "ObjectBlockSurpriseYellow",
    "ObjectBlockRaceStart",
    "ObjectSoundBlinkBlock",
    "ObjectTimerSwitchBlockSync",
    "ObjectSinkBlock",
    "ObjectAshibaDisappearStep",
    "ObjectBlockLift",
    "ObjectDokan",
    "ObjectXylophoneBridge",
    "AreaGelField",
    "ObjectPropellerFlowerForCourse",
    "ObjectBlockHotStone",
    "ObjectGoQKun"
]