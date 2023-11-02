from PyQt5.QtGui import *
from PyQt5.QtWidgets import *
from PyQt5.QtCore import *
import yaml
import wrapt
from yaml import Loader, Dumper

class Tagged(wrapt.ObjectProxy):
    # tell wrapt to set the attribute on the proxy, not the wrapped object
    tag = None

    def __init__(self, tag, wrapped):
        super().__init__(wrapped)
        self.tag = tag

    def __repr__(self):
        return f"{type(self).__name__}({self.tag!r}, {self.__wrapped__!r})"

def construct_undefined(self, node):
    if isinstance(node, yaml.nodes.ScalarNode):
        value = self.construct_scalar(node)
    elif isinstance(node, yaml.nodes.SequenceNode):
        value = self.construct_sequence(node)
    elif isinstance(node, yaml.nodes.MappingNode):
        value = self.construct_mapping(node)
    else:
        assert False, f"unexpected node: {node!r}"
    return Tagged(node.tag, value)

Loader.add_constructor(None, construct_undefined)

def represent_tagged(self, data):
    assert isinstance(data, Tagged), data
    node = self.represent_data(data.__wrapped__)
    node.tag = data.tag
    return node

Dumper.add_representer(Tagged, represent_tagged)

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

def resize_label(label):
    target_rect = label.contentsRect()
    text = label.text()

    # Use binary search to efficiently find the biggest font that will fit.
    max_size = label.height()
    min_size = 1
    font = label.font()
    while 1 < max_size - min_size:
        new_size = (min_size + max_size) // 2
        font.setPointSize(new_size)
        metrics = QFontMetrics(font)

        # Be careful which overload of boundingRect() you call.
        rect = metrics.boundingRect(target_rect, Qt.AlignLeft, text)
        if (rect.width() > target_rect.width() or
                rect.height() > target_rect.height()):
            max_size = new_size
        else:
            min_size = new_size

    font.setPointSize(min_size)
    label.setFont(font)