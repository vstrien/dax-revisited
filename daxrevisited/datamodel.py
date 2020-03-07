from daxrevisited.table import Table

class Datamodel:
    def __init__(self):
        self.tables = []
        self.relations = []

    def add_table(self):
        self.tables.append(Table)