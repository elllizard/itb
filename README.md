# itb

[Start Telegram Bot](https://t.me/i_g_t_g_bot)

## Build

#### Build docker image ready for deployment using `Docker`:

```
    docker build -t itb .
```

#### Login to `Heroku` using `Heroku CLI`:

```
    heroku container:login
```

#### Push docker image to `Heroku`:

```
    heroku container:push --app=insttgbot web
```

#### Start pushed docker image on `Heroku`:

```
    heroku container:release --app=insttgbot web
```