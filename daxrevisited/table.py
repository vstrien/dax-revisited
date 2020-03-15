class Table:
    """One or more columns with datatypes"""

    class Column:
        """A column with a datatype and possibly data"""
        
        POSSIBLE_DATA_TYPES = ["binary", "boolean", "currency", "datetime", "decimal", "integer", "string", "variant"]

        def __init__(self, datatype):
            if datatype not in self.POSSIBLE_DATA_TYPES:
                raise self.InvalidDatatypeError("{0} is not a valid datatype. Valid datatypes are: {1}".format(datatype, ", ".join(self.POSSIBLE_DATA_TYPES)))
            self.datatype = datatype
            self.values = []

        @classmethod
        def from_values(cls, datatype: str = "string", values = []):
            newColumn = cls(datatype)
            newColumn.values = values
            return newColumn

        class InvalidDatatypeError(Exception):
            """Exception raised when data type is not valid for this column"""
            def __init__(self, message):
                self.message=message

    def __init__(self, columns: dict = {}):
        self.columns = columns.copy()
        
        self.n_rows = 0
        try:
            self.n_rows = len(next(iter(self.columns.values())))
        except StopIteration:
            pass
        

    def add_column(self, column_name, datatype: str = "string", default_value = ""):
        """Add a column to the table. Every row should have a value"""
        self.columns[column_name] = self.Column(datatype)

        for i in range(self.n_rows):
            self.columns[-1].values[i] = default_value

    def add_column_with_values(self, column_name, datatype: str = "string", values: list = []):
        """Add a column to the table"""
        #print("{0}, {1}".format(datatype, ",".join(values)))
        newColumn = self.Column.from_values(datatype, values)
        self.columns[column_name] = newColumn
