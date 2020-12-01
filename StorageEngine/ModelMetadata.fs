namespace ModelMetadata

type DataAccessOptions = {
    legacyRedirects: bool
    returnErrorValuesAsNull: bool
}

type AttributeHierarchy = {
    state: string
    modifiedTime: string
    refreshedTime: string
}

type Annotation = {
    name: string
    value: string
    modifiedTime: string
}

type Column = {
    name: string
    dataType: string
    sourceColumn: string
    formatString: string
    summarizeBy: string
    modifiedTime: string
    structureModifiedTime: string
    refreshedTime: string
    attributeHierarchy: AttributeHierarchy
    annotations: list<Annotation>
}

type PartitionSource = {
    mytype: string
    expression: string
}

type Partition = {
    name: string
    mode: string
    state: string
    modifiedTime: string
    refreshedTime: string
    source: PartitionSource
}

type Table = {
    name: string
    modifiedTime: string
    structureModifiedTime: string
    columns: list<Column>
    partitions: list<Partition>
    annotations: list<Annotation>
}

type Relationship = {
    name: string
    fromTable: string
    fromColumn: string
    toTable: string
    toColumn: string
    state: string
    modifiedTime: string
    refreshedTime: string
}

type LinguisticMetadataContent = {
    Version: string
    Language: string
    DynamicImprovement: string
}

type LinguisticMetadata = {
    content: LinguisticMetadataContent
    contentType: string
    modifiedTime: string
}

type Culture = {
    name: string
    modifiedTime: string
    structureModifiedTime: string

}


type DataModel = {
    culture: string
    dataAccessOptions: DataAccessOptions
    defaultPowerBIDataSourceVersion: string
    sourceQueryCulture: string
    modifiedTime: string
    structureModifiedTime: string
    tables: list<Table>
    relationships: list<Relationship>
    cultures: list<Culture>
    annotations: list<Annotation>
}

type DataModelSchema = {
    name: string
    compatibilitylevel: int
    createdTimestamp: string 
    lastUpdate: string
    lastSchemaUpdate: string
    lastProcessed: string
    model: DataModel
}