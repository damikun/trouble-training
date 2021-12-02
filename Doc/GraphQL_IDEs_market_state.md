
There are a few GraphQL IDEs on the market today that support various features, but I am not satisfied with any of them. (True state)

These tools should be our internal applications/tools and make it easier to work with GraphQL!

## Todays GraphQL IDs

- `Graphiql` - Facebook's oldest original still looks like it's from the 90s and does not support tabs. The tool itself is great because it's stable and provides separate packages under Monorepo that are used by different tools. Let us say it's a tool used to build other tools :)
- `Playgorund` - The good old playgorund was widely used. It was developed by Prisma, but then moved under Graphiql (Graphql Foundation). With that, the project was discontinued and there is nothing as simple and good anymore. Playground was just good for the old days.
- `Appolo Studio` - Appolo has several products to work with, sometimes I do not even know what is deprecated and what is current (Devtools, Studio and whatever) have you tried it? No relay my taste. It's like with the Appolo client, nice to look at but then you realize there's Relay too :) But they have advanced functionality that is true!
- `Altari` - No Relay good UX experience
- `Insomnia` - I was surprised by the functionality. I like plugins a lot. It is close to the requirements I will talk about.
- `Firecamp` - Another IDE, which has the possibility of bee good if they implement what they have planned. But it is not yet finished!
- `And whatever else` - I am just not happy with it.

Let me mention one other option that exists. The `Postman`.

Postman as GraphQL IDE? Not yet. But I see the possibility of it becoming a powerfool tool for GraphQL. 

This is something I do not understand. They have a great feature set and also support the basics of GraphQL, but they just do not care! Important features like introspection are not supported. If they would invest more time, they could become a gamechanger, but they just do not care. (Check out the PRs on Graphql features on Github).

But back to the topic: if you are a business, you'll probably end up building your own tool.

## The requirements

- Readable UI experience and easy to work with (most current IDEs are garbage for this UX theme)
- Ability to store your state in the cloud / locally / on your own backend (having your own storage is a really good idea, especially if you are in the private sector)
- Prepare test collections, import/export work with them.
- Headless cli to be able to run collections in CI/CD.
- Voyager built in! Why not? I do not want to have multiple tools.
- You want to monitor your requests and view telemetry data, you want to understand your requests.
- Auth, SSL are standard requirements today
- Query optimization, maybe too much, but it works.

The company ends up developing a custom tool to get all this done, because it's really not hard to do. (Only time matters). We use `Graphiql` as a base and everything else is just WebApp/cli programming as we know it from our daily work.

Keep things under control.