from daxrevisited.table import Table

class Datamodel:
    def __init__(self):
        self.tables = {}
        self.relations = []

    def add_table(self, tablename):
        self.tables[tablename] = Table()

    def add_table_with_data(self, tablename, columnnames, datatypes, data):
        self.tables[tablename] = Table()
        for columnname, datatype, contents in zip(columnnames, datatypes, data):
            self.tables[tablename].add_column_with_values(columnname, datatype, contents)