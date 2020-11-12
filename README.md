# DAX Revisited

Waaay to ambitious, but still.

I've walked around with this idea for quite some time, so why not give it a shot.

Would it be possible to build a DAX engine? You know, like the one used in Power BI or Analysis Services. Not to compete or improve, but to better "get" the inner workings. And to have it cross-platform 😈.

Just some aspects I've been thinking of:

* Microsoft has built Power Pivot (looong time ago) on a "converted" SQL Server Analysis Services engine. 
  * We could start off using another (open source) OLAP engine
  * But hey, that would probably make it more complex
  * Although it might be an opportunity to learn more about OLAP engines
  * But no. For me currently it's not about speed, but about the evaluation of the DAX language
* DAX is a functional language (not in the general-purpose way Power Query is - which resembles OCaml and thus F# - but still it's a functional language). Should I implement this in a functional language?
  * Performance might be hurt (some things just are "dirty" somewhat faster)
  * But performance isn't the primary concern
  * Would be great to brush off functional programming skills as well
* Which language? (functional or not)
  * First attempt: Python. Because I've loads of experience in Python.
  * Second thought: use a Microsoft-language that can use .Net
    * open source shouldn't be a problem (.Net Core)
    * When I haven't implemented a part myself, I can use the external interface from other software
      * For example, use an existing Power BI or SSAS library to see the external interface
      * Or get a headstart by using existing functionality in Tabular Editor
  * Microsoft-language + open source = F#?
* A good first step would be to load an existing PBIT-file.
  * Tabular Editor 
    * Is licensed under MIT
    * Has a "wrapper" for the [Tabular Object Model (TOM)](https://github.com/otykier/TabularEditor/tree/master/TOMWrapper/TOMWrapper)
    * Can be useful for structure, although it uses the Microsoft.AnalysisServices.Tabular library on several places (which is windows-only)
    * As an extensive tool, it also has all kinds of features (like partitions) we don't need to build a working DAX core.
  * ALM Toolkit (BISM Normalizer) is open-source as well (?)
    * Also has some information about [a data model](https://github.com/microsoft/Analysis-Services/tree/master/BismNormalizer/BismNormalizer/TabularCompare/TabularMetadata) (note that it's not aimed at storing data, but still tells something about the metadata)
    * Is licensed under MIT license
    * Uses Microsoft.AnalysisServices.Tabular library on several places as well (which is still win-only)
  * If we can "load" an existing PBIT-file, then we have a "fitting" way to store our data model and measures (apart from the data)
* A second step would be to load data itself
  * Rebuilding columnar storage shouldn't be as hard. Conceptually, at least. But even if we don't, we just have to store it.
  * If it's possible (not encrypted) load it from a Power BI file
  * Otherwise think of a way to load data directly (maybe from a SQL Server refresh, bypassing Power Query and likewise tools)
* Then we could implement simple query concepts like `EVALUATE SUM('Table'[Column])`
* Where the truly interesting stuff happens is experiencing context switches from a programming side. We could a lot from that.

Om dit enigszins succesvol te kunnen laten zijn, is het belangrijk om test-driven te beginnen. De verleiding is groot om direct van start te gaan, een database-verbinding op te leren zetten en data in te tanken. Dat is zeker interessant voor grootschalige(re) testen, maar niet het belangrijkste: het gaat om het query parsen (het "tot resultaat komen"), dat je juist met zo klein mogelijke datasets wilt kunnen doen.

Bijkomend voordeel is dat het een mooie kans is om beter test-driven te leren werken.

## Hoogover ontwerp

Het voornaamste zal een stukje formula parsing zijn. Hierbij weet ik dat DAX aan de achterkant een "storage engine" en een "formula engine" gebruikt - waarbij de kunt is zoveel mogelijk door de "storage engine" te laten doen. Wat dat betreft niet zoveel anders dan relationele database-technieken dus (het slim uitvoeren van queries).

Als eerste stap zou ik graag een datastructuur hebben voor een datamodel, met daarin tabellen en relaties. De tabellen hebben kolommen, en in de kolommen zit de data (ik denk aan een lijst van waarden, waarbij het datatype en andere eigenschappen op object-niveau gezet zijn).

In deze eerste versie maken we ons niet druk over performance, in-memory compressie e.d.. De tabel moet echter, gegeven een datastructuur, een "tabel" aangeboden kunnen krijgen en deze inladen in de onderliggende kolommen.

Nadat er data ingeladen kan worden, is de tweede stap de data er weer uit halen zonder onder water alle rijen te benaderen. Kortom: er moet een *dax interpreter* komen die de DAX interpreteert. Nu is DAX een functionele taal, dus elke expressie is relatief eenvoudig om te schrijven naar een stack. De verantwoordelijkheid van de DAX Interpreter wordt om de DAX-expressies te begrijpen, op te splitsen in onderliggende acties, en de resultaten terug te geven.

Deze DAX interpreter kan in beginsel al bestaan los van het datamodel: 3 + 3 is ook een geldige DAX-expressie. Tegelijk is er al vrij snel geen sprake meer van constanten - en er is een groot grijs gebied. Hoe werkt een expliciet opgestelde tabel bijvoorbeeld - wordt die onder water tijdelijk in geheugen gehouden, maar wel met een "echte" structuur?