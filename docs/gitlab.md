# GitLab

GitLab is used to run the CI (Continuous Integration) pipeline, which is defined in `.gitlab-ci.yml` file.

## Setup

### CI/CD > Schedules

* Add new schedule to run every night

### Settings > CI/CD > Variables

* Add 4 variables

Name | Value
---- | -----
NUGET_APIKEY | API key generated from nuget.org website
SERVICENOW_SANDBOX_URL | ServiceNow Sandbox URL
SERVICENOW_SANDBOX_USERNAME | ServiceNow Sandbox Username
SERVICENOW_SANDBOX_USERPWD | ServiceNow Sandbox Password

## Debug

### Run locally the pipeline

* Workaround: `include` do not work with local runner so in case of issue copy/paste the pipeline content and follow the steps below

* Make sure all content files to be checked are committed (except `.gitlab-ci.yml`)

* Run the runner locally with Docker

```bash
# creates local folder
mkdir -p .gitlab/runner/local

# runs build job
docker run --rm --name gitlab-runner --workdir $PWD \
  -v /var/run/docker.sock:/var/run/docker.sock \
  -v $PWD/.gitlab/runner/local/config:/etc/gitlab-runner \
  -v $PWD:$PWD \
  gitlab/gitlab-runner exec docker build

# runs test job

docker run --rm --name gitlab-runner --workdir $PWD \
  -v /var/run/docker.sock:/var/run/docker.sock \
  -v $PWD/.gitlab/runner/local/config:/etc/gitlab-runner \
  -v $PWD:$PWD \
  gitlab/gitlab-runner exec docker \
    --env SERVICENOW_SANDBOX_URL=*** \
    --env SERVICENOW_SANDBOX_USERNAME=*** \
    --env SERVICENOW_SANDBOX_USERPWD=*** \
    --env SONAR_ORGANIZATION=*** \
    --env SONAR_PROJECTKEY=*** \
    --env SONAR_HOSTURL=*** \
    --env SONAR_TOKEN=*** \
    test

# runs pack job
docker run --rm --name gitlab-runner --workdir $PWD \
  -v /var/run/docker.sock:/var/run/docker.sock \
  -v $PWD/.gitlab/runner/local/config:/etc/gitlab-runner \
  -v $PWD:$PWD \
  gitlab/gitlab-runner exec docker \
    --env CI_COMMIT_BRANCH=feature/init-solution \
    --env CI_PIPELINE_ID=1234 \
    --env NUGET_APIKEY=*** \
    pack
```
