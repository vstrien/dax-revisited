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
        del(t)

    def test_columns_should_have_valid_datatypes(self):
        """Try to get some nonsensical datatypes accepted"""
        t = Table()
        self.assertRaises(Table.Column.InvalidDatatypeError, t.add_column, "foo")
        del(t)

    def test_add_column_with_values_to_table(self):
        """Add a column with values to the table"""
        t = Table()
        t.add_column_with_values("string", ["a", "b", "c"])
        del(t)

    def test_add_column_with_invalid_datatype_to_table(self):
        """Try to add a column with not-existing datatype"""
        t = Table()
        self.assertRaises(Table.Column.InvalidDatatypeError, t.add_column_with_values, "foo", ["a", "b", "c"])
        del(t)

    def test_retrieve_data_from_table(self):
        """After adding two columns with three rows, data should come out as well"""
        t = Table()
        t.add_column_with_values("string", ["a", "b", "c"])
        t.add_column_with_values("integer", [1, 2, 3])
        self.assertEqual(t.columns[0].values[1], "b")
        self.assertEqual(t.columns[1].values[2], 3)
        del(t)

    def test_after_deleting_and_recreating_new_table_should_be_empty(self):
        """Python should copy the lists provided (not just link them). Otherwise, newly created tables might suddenly contain data."""
        t = Table()
        t.add_column_with_values("string", ["a", "b", "c"])
        del(t)
        t = Table()
        self.assertEqual(len(t.columns), 0)

class TestDatamodel:
    def test_create_datamodel(self):
        """It should be possible to create an empty data model"""
        Datamodel()
    
    def test_add_table_to_datamodel(self):
        """Create a datamodel, add a table"""
        d = Datamodel()
        d.add_table()