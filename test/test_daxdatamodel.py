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
        t.add_column("column1")
        del(t)

    def test_columns_should_have_valid_datatypes(self):
        """Try to get some nonsensical datatypes accepted"""
        t = Table()
        self.assertRaises(Table.Column.InvalidDatatypeError, t.add_column, "column1", "foo")
        del(t)

    def test_add_column_with_values_to_table(self):
        """Add a column with values to the table"""
        t = Table()
        t.add_column_with_values("column1", "string", ["a", "b", "c"])
        del(t)

    def test_add_column_with_invalid_datatype_to_table(self):
        """Try to add a column with not-existing datatype"""
        t = Table()
        self.assertRaises(Table.Column.InvalidDatatypeError, t.add_column_with_values, "column1", "foo", ["a", "b", "c"])
        del(t)

    def test_retrieve_data_from_table(self):
        """After adding two columns with three rows, data should come out as well"""
        t = Table()
        t.add_column_with_values("column1", "string", ["a", "b", "c"])
        t.add_column_with_values("column2", "integer", [1, 2, 3])
        self.assertEqual(t.columns["column1"].values[1], "b")
        self.assertEqual(t.columns["column2"].values[2], 3)
        del(t)

    def test_after_deleting_and_recreating_new_table_should_be_empty(self):
        """Python should copy the lists provided (not just link them). Otherwise, newly created tables might suddenly contain data."""
        t = Table()
        t.add_column_with_values("column1", "string", ["a", "b", "c"])
        del(t)
        t = Table()
        self.assertEqual(len(t.columns), 0)

class TestDatamodel(unittest.TestCase):
    def test_create_datamodel(self):
        """It should be possible to create an empty data model"""
        Datamodel()
    
    def test_add_table_to_datamodel(self):
        """Create a datamodel, add a table"""
        d = Datamodel()
        d.add_table("My Test Table")

    def test_add_table_with_data_to_datamodel(self):
        """Create a datamodel, add a table with values"""
        d = Datamodel()
        d.add_table_with_data("My Test Table", ["id", "productname"], ["integer", "string"], [[1, 2, 3, 4, 5], ["product1", "product2", "product3", "product4", "product5"]])

    ## todo: tables with columns with inequal number of rows should throw an error

    ## todo: Adding a relationship should work (happy flow)

    ## todo: Adding a relationship with duplicate values on the "one" side should throw an error

    ## todo: Adding a relationship with missing values on the "one" side should throw an error


## todo: class for low-level computations (more or less the "storage engine" for DAX - everything that can be handled on the column)

## todo: class for high-level computations (the "formula engine" in DAX)

