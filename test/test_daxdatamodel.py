from daxrevisited.datamodel import *
from daxrevisited.table import *
import unittest

class TestTable(unittest.TestCase):
    def test_create_empty_table(self):
        """It should be possible to create an empty table"""
        Table()

    def test_add_column_to_table(self):
        """Add a column to a table"""
        t = Table()
        t.add_column()

    def test_columns_should_have_valid_datatypes(self):
        """Try to get some nonsensical datatypes accepted"""
        t = Table()
        self.assertRaises(Table.Column.InvalidDatatypeError, t.add_column, "hoi")

class TestDatamodel:
    def test_create_datamodel(self):
        """It should be possible to create an empty data model"""
        Datamodel()
    
    def test_add_table_to_datamodel(self):
        """Create a datamodel, add a table"""
        d = Datamodel()
        d.add_table()