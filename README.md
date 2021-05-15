![Discord Bot Logo](https://i.imgur.com/1t5cGfM.png)

# Overview

A custom bot made to enhance and moderate the user experience on the official Discord server for MonoGame.

[Join the server.](https://discord.gg/tSJz7gD)

## Technologies
- [C#](https://docs.microsoft.com/en-us/dotnet/csharp/tour-of-csharp/), [ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-5.0), and [.NET Core 3.1](https://dotnet.microsoft.com/download/dotnet/3.1)
- [Docker](https://www.docker.com/)
- [MongoDB](https://mongodb.org)

## Dependencies
- [Discord.Net](https://github.com/discord-net/Discord.Net)
- [Docker](https://www.docker.com/) (i.e. `docker-compose` and `docker`), with BuildKit enabled.
- [MongoDB](https://www.mongodb.com/) (deployable through Docker)
- Windows Subsystem for Linux 2 (WSL2)
- [Bootstrap 4.3](https://getbootstrap.com/)

### Why Bootstrap?

This is a powerful and robust CSS and JS framework that is used for building modern and responsive web applications. This is used primarily for the web-based control panel that is to be developed at a later date. The source for Bootstrap has been committed as part of the source code for the bot so that it can be customised (e.g. injected with MonoGame branding colours).

### Why Docker?

Following on the principle of "infrastructure as code", Docker provides a convenient means for both developing server-sided software in a local environment at the stroke of a few simple commands (i.e. `docker-compose -f development build --no-cache` and `docker-compose -f development up`). Future releases of the bot can be deployed as a custom docker container through a private registry that is maintained by the owners of this repository.

For more information about our Docker configuration, view the `docker-compose` and `Dockerfile` definitions under `Docker/` in this repository.

### Why MongoDB?

This is used primarily for data persistence and event logging. Should the bot have to be restarted, it's important that it has all relevant data including user and channel information, so that it can persist the state that it was in before.

## Goals

A small list of things that we'd like to achieve with this bot.

### Commands

- [ ] TBA

---

### Features

- [ ] **Welcome New Users**
- [ ] **Community Polling**
  - [ ] Ability to start a new poll with multiple choices
  - [ ] Vote on a choice using defined "reactions" 
- [ ] **Q&A System**
  - [ ] Ability to register a new question to ask to other users.
- [ ] **MonoGame Showcase Submission**
  - [ ] Request new showcase submissions using an attached link or content. 

### Technical

- [x] Docker deployable bot.
- [x] Remotely administrated from a web-based control panel.
- [ ] Support for Docker sharding with Kubernetes support.
