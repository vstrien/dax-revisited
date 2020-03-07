# DAX Revisited

Veel te ambitieus, maar toch:

Zou het mogelijk zijn om een DAX engine te bouwen? Gewoon, eentje die een deel van de taal begrijpt? Niet om te concurreren of te verbeteren, maar om de werking beter te snappen. En om 'm cross-platform te hebben 😈.

Een paar aspecten waar ik aan denk:

* Columnar storage nabouwen (en de uitdagingen / mogelijkheden daarvan meemaken)
* Interpreteren van eenvoudige DAX query concepten als EVALUATE SUM('Tabel'[Kolom])
* Uitdagingen van contexten ondervinden vanuit de programmeer-kant

Om dit enigszins succesvol te kunnen laten zijn, is het belangrijk om test-driven te beginnen. De verleiding is groot om direct van start te gaan, een database-verbinding op te leren zetten en data in te tanken. Dat is zeker interessant voor grootschalige(re) testen, maar niet het belangrijkste: het gaat om het query parsen (het "tot resultaat komen"), dat je juist met zo klein mogelijke datasets wilt kunnen doen.

Bijkomend voordeel is dat het een mooie kans is om beter test-driven te leren werken in Python. En laat dat nou net zijn wat ik graag wilde ;-).

## Hoogover ontwerp

Het voornaamste zal een stukje formula parsing zijn. Hierbij weet ik dat DAX aan de achterkant een "storage engine" en een "formula engine" gebruikt - waarbij de kunt is zoveel mogelijk door de "storage engine" te laten doen. Wat dat betreft niet zoveel anders dan relationele database-technieken dus (het slim uitvoeren van queries).

Als eerste stap zou ik graag een datastructuur hebben voor een datamodel, met daarin tabellen en relaties. De tabellen hebben kolommen, en in de kolommen zit de data (ik denk aan een lijst van waarden, waarbij het datatype en andere eigenschappen op object-niveau gezet zijn).

In deze eerste versie maken we ons niet druk over performance, in-memory compressie e.d.. De tabel moet echter, gegeven een datastructuur, een "tabel" aangeboden kunnen krijgen en deze inladen in de onderliggende kolommen.

Nadat er data ingeladen kan worden, is de tweede stap de data er weer uit halen zonder onder water alle rijen te benaderen. Kortom: er moet een *dax interpreter* komen die de DAX interpreteert. Nu is DAX een functionele taal, dus elke expressie is relatief eenvoudig om te schrijven naar een stack. De verantwoordelijkheid van de DAX Interpreter wordt om de DAX-expressies te begrijpen, op te splitsen in onderliggende acties, en de resultaten terug te geven.

Deze DAX interpreter kan in beginsel al bestaan los van het datamodel: 3 + 3 is ook een geldige DAX-expressie. Tegelijk is er al vrij snel geen sprake meer van constanten - en er is een groot grijs gebied. Hoe werkt een expliciet opgestelde tabel bijvoorbeeld - wordt die onder water tijdelijk in geheugen gehouden, maar wel met een "echte" structuur?