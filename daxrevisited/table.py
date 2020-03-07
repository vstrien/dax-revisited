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

    def __init__(self, columns: list = []):
        self.columns = columns.copy()
        self.n_rows = len(self.columns[0].values) if len(self.columns) > 0 else 0

    def add_column(self, datatype: str = "string", default_value = ""):
        """Add a column to the table. Every row should have a value"""
        self.columns.append(self.Column(datatype))

        for i in range(self.n_rows):
            self.columns[-1].values[i] = default_value

    def add_column_with_values(self, datatype: str = "string", values: list = []):
        """Add a column to the table"""
        #print("{0}, {1}".format(datatype, ",".join(values)))
        newColumn = self.Column.from_values(datatype, values)
        self.columns.append(newColumn)
