# UpDownSlack
App that check if a number of endpoints are up, down or slow and post the result to a slack channel.


docker build
```sh
docker build -t up-down-slack -f Dockerfile . 
```

copy example.env file (and edit it's content)
```sh
cp example.env .env 
```

run the docker container
```sh
docker run --rm --env-file .env up-down-slack 
```
