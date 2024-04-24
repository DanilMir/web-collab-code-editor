#!/bin/bash

npx y-websocket > y-websocket.log 2>&1
/server > server.log 2>&1

tail -f *.log