class Table:
    """One or more columns with datatypes"""

    class Column:
        POSSIBLE_DATA_TYPES = ["binary", "boolean", "currency", "datetime", "decimal", "integer", "string", "variant"]

        """A column with a datatype and possibly data"""
        def __init__(self, datatype):
            if datatype not in self.POSSIBLE_DATA_TYPES:
                raise self.InvalidDatatypeError("{0} is not a valid datatype. Valid datatypes are: {1}".format(datatype, ", ".join(self.POSSIBLE_DATA_TYPES)))
            self.datatype = datatype
            self.values = []

        class InvalidDatatypeError(Exception):
            """Exception raised when data type is not valid for this column"""
            def __init__(self, message):
                self.message=message

    def __init__(self, columns: list = []):
        self.columns = columns
        self.n_rows = len(self.columns[0].values) if len(self.columns) > 0 else 0

    def add_column(self, datatype: str = "string", default_value = ""):
        """Add a column to the table. Every row should have a value"""
        self.columns.append(self.Column(datatype))

        for i in range(self.n_rows):
            self.columns[-1].values[i] = default_value