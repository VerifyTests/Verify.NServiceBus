# <img src="/src/icon.png" height="30px"> Verify.NServiceBus

[![Build status](https://ci.appveyor.com/api/projects/status/wwrri8srggv1h56j/branch/master?svg=true)](https://ci.appveyor.com/project/SimonCropp/Verify-NServiceBus)
[![NuGet Status](https://img.shields.io/nuget/v/Verify.NServiceBus.svg)](https://www.nuget.org/packages/Verify.NServiceBus/)

Adds [Verify](https://github.com/SimonCropp/Verify) support to verify [NServiceBus Test Contexts](https://docs.particular.net/nservicebus/samples/unit-testing/).

toc

<!--- StartOpenCollectiveBackers -->

[Already a Patron? skip past this section](#endofbacking)


## Community backed

**It is expected that all developers [become a Patron](https://opencollective.com/nservicebusextensions/order/6976) to use any of these libraries. [Go to licensing FAQ](https://github.com/NServiceBusExtensions/Home/blob/master/readme.md#licensingpatron-faq)**


### Sponsors

Support this project by [becoming a Sponsors](https://opencollective.com/nservicebusextensions/order/6972). The company avatar will show up here with a link to your website. The avatar will also be added to all GitHub repositories under this organization.


### Patrons

Thanks to all the backing developers! Support this project by [becoming a patron](https://opencollective.com/nservicebusextensions/order/6976).

<img src="https://opencollective.com/nservicebusextensions/tiers/patron.svg?width=890&avatarHeight=60&button=false">

<!--- EndOpenCollectiveBackers -->

<a href="#" id="endofbacking"></a>


## Usage

Before any test have run call:

```
VerifyNServiceBus.Enable();
```


### Verifying a context

Given the following handler:

snippet: SimpleHandler

The test that verifies the resulting context:

snippet: HandlerTest

The resulting context verification file is as follows:

snippet: MessageHandlerTests.VerifyHandlerResult.verified.txt


### Example behavior change

The next time there is a code change, that results in a different resulting interactions with NServiceBus, those changes can be visualized. For example if the `DelayDeliveryWith` is changed from 12 hours to 1 day:

snippet: SimpleHandlerV2

Then the resulting visualization diff would look as follows:


![visualization diff](/src/approvaltests-diff.png)


## Release Notes

See [closed milestones](../../milestones?state=closed).


## Icon

[Approval](https://thenounproject.com/term/approval/1759519/) designed by [Mike Zuidgeest](https://thenounproject.com/zuidgeest/) from [The Noun Project](https://thenounproject.com/).