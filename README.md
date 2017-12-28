# Gander

In this project, I'm exploring feasibility of a browser testing framework using [Selenium](http://www.seleniumhq.org/). I've played around [Ghost Inspector](https://ghostinspector.com/), a browser testing service, and I think it's pretty cool. But I have some discomforts with the record and playback model since you have to record each test manually, and I'm concerned that this doesn't scale very well to large apps. My other concern is that pass/fail has to be done by "scraping" web pages for evidence. It would be simpler to be able to query the back-end database directly. So that's what I want to achieve with this -- some sort of scale advantage for testing large apps, and database-level assertions for pass/fail detection. The "scale advantage" I'm referring to means the ability to generate use cases dynamically as well as to empower the developer ("shift left") with some kind of declarative shorthand.

I have to say I'm not a huge fan of Test-Driven Development. Although I believe there are good scenarios for it, in my experience it's just too intrusive and complicated. But I *do* recognize a need for test automation of some kind -- especially at the browser level since that's what the user experiences. As apps get larger and more complicated, manual testing becomes a bottleneck to completing fixes and enhancements. As time goes by, a creeping dread of breaking apps with each deployment has a chilling effect on productivity. I have lived this!

As of now (12/28/17), I'm just drafting the low-level model and XML serialization, and just barely getting into Selenium for the first time.