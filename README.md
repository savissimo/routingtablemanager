# Routing Table Manager

A tool to quickly add entries to the Windows routing table. Host names are resolved with DNS for user convenience. 

## Features

* Routing entries may specify a destination and a gateway either as an IP address or as a host name, which will be resolved with DNS
* Routing entries can point to a specific network interface
* Projects contain multiple routing entries, so you can group them and enable them at once 
* Command-line parameter allows to apply all the entries of a project without opening the UI
* It needs a Windows account with elevated privileges

## Project status

The tool is now functional, at least as to what I need it for. It was meant as a quick and convenient way to add entries to the system routing table, and that it does. 

**I don't plan to refine it any further**. I have no interest in new features or in a nicer UI, and I am caught in several other projects. 

### "Then why did you put it here?!"

Because I'm a nice guy ;)

Just because I have no plans for this project, it doesn't mean _you_ have none! I decided to open this project for three main reasons:
1. so that people can use it, if they need to. They can't if it sits on my hard drive;
2. so that people can point out problems. What works for me may not work for you; 
3. so that people can pick up from here and improve it themselves :)
