#! /bin/bash

set -e

IMAGE=$(docker build -q -f ./AotResilience.Cli/Dockerfile .)
MSYS_NO_PATHCONV=1 docker run --rm $IMAGE
