module App.Tests

open NUnit.Framework
open PbitReader

[<TestFixture>]
type TestClass () = 
    [<SetUp>]
    member this.Setup () =
        ()

    [<Test>]
    member this.TestMethodPassing() =
        Assert.True(true)

    [<Test>]
    member this.TestQueryDistinctCities() = 
        let daxquery = "EVALUATE DISTINCT('City'[City])"
        Assert.Fail()

    [<Test>]
    member this.TestTop10Cities() = 
        let daxquery = "EVALUATE TOPN(10, DISTINCT('City'[City]), 'City'[City], 1)"
        Assert.Fail()

    [<Test>]
    member this.TestTop100Cities() = 
        let daxquery = "EVALUATE TOPN(100, DISTINCT('City'[City]), 'City'[City], 1)"
        Assert.Fail()
 
    [<Test>]
    member this.UseVariable() = 
        let daxquery = "DEFINE
        VAR __DS0Core = 
            DISTINCT('City'[City])
        
        VAR __DS0BodyLimited = 
            TOPN(3502, __DS0Core, 'City'[City], 1)
        
        EVALUATE
            __DS0BodyLimited"
        Assert.Fail()

        
    [<Test>]
    member this.OrderResults() = 
        let daxquery = "DEFINE
        VAR __DS0Core = 
            DISTINCT('City'[City])

        VAR __DS0BodyLimited = 
            TOPN(3502, __DS0Core, 'City'[City], 1)

        EVALUATE
            __DS0BodyLimited

        ORDER BY
            'City'[City]"
        Assert.Fail()
    
    // 4b) add descending orders
    // 4c) do multiple-column ordering as well
    
    [<Test>]
    member this.SummarizeColumns() = 
        let daxquery = "EVALUATE SUMMARIZECOLUMNS(
          
          \"SumUnit_Price\", CALCULATE(SUM('Sale'[Unit Price]))
        )"
        Assert.Fail()
    
    [<Test>]
    member this.RollupAddIsSubtotal() = 
        let daxquery = "EVALUATE SUMMARIZECOLUMNS(
          ROLLUPADDISSUBTOTAL('City'[City], \"IsGrandTotalRowTotal\"),
          \"SumUnit_Price\", CALCULATE(SUM('Sale'[Unit Price]))
        )"
        Assert.Fail()

        
    // This one needs to be split up.
    // Currently not only about multiple result sets, but also NATURALLEFTOUTERJOIN and SUBSTITUTEWITHINDEX
    // Those functions need to be tested separately
    [<Test>]
    member this.MultipleResultSets() =
        let daxquery = "DEFINE
          VAR __DS0Core = 
            SUMMARIZECOLUMNS(
              ROLLUPADDISSUBTOTAL('City'[City], \"IsGrandTotalRowTotal\"),
              ROLLUPADDISSUBTOTAL('Customer'[Category], \"IsGrandTotalColumnTotal\"),
              \"AverageUnit_Price\", CALCULATE(AVERAGE('Sale'[Unit Price]))
            )

          VAR __DS0PrimaryWindowed = 
            TOPN(
              102,
              SUMMARIZE(__DS0Core, 'City'[City], [IsGrandTotalRowTotal]),
              [IsGrandTotalRowTotal],
              0,
              'City'[City],
              1
            )

          VAR __DS0SecondaryBase = 
            SUMMARIZE(__DS0Core, 'Customer'[Category], [IsGrandTotalColumnTotal])

          VAR __DS0Secondary = 
            TOPN(102, __DS0SecondaryBase, [IsGrandTotalColumnTotal], 1, 'Customer'[Category], 1)

          VAR __DS0BodyLimited = 
            NATURALLEFTOUTERJOIN(
              __DS0PrimaryWindowed,
              SUBSTITUTEWITHINDEX(
                __DS0Core,
                \"ColumnIndex\",
                __DS0Secondary,
                [IsGrandTotalColumnTotal],
                ASC,
                'Customer'[Category],
                ASC
              )
            )

        EVALUATE
          __DS0Secondary

        ORDER BY
          [IsGrandTotalColumnTotal], 'Customer'[Category]

        EVALUATE
          __DS0BodyLimited

        ORDER BY
          [IsGrandTotalRowTotal] DESC, 'City'[City], [ColumnIndex]"
        Assert.Fail()