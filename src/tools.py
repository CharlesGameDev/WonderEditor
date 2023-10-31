from PyQt5.QtGui import *
from PyQt5.QtWidgets import *
from PyQt5.QtCore import *

def make_action(name: str, parent: QObject, slot = None, shortcut: str = "", checkable: bool = False, default_check=False) -> QAction:
    action = QAction(name, parent, checkable=checkable)
    action.setChecked(default_check)
    if action != None:
        action.triggered.connect(slot)
    if shortcut != "":
        action.setShortcut(QKeySequence(shortcut))

    return action

def check_contains(key, list):
    for item in list:
        if item == key: return True
    return False