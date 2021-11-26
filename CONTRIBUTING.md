# Rules when contributing to the project:

## Branch naming

Try to be descriptive. Use the following prefixes for the names depending on the type of work:

* feature/ for new features.
* bug/ for bugfixes.
* doc/ for documentation.
* test/ for testing.
* debt/ for refactoring and enhancements/improvements.

## Commit messages

In English, not too long. Additional information after two newlines.

## Contribution flow

```
# Create and checkout a new branch for the contribution
$ git checkout -b doc/contributing-guidelines
# Make your changes
$ vim README.md
# Commit changes
$ git add . && git commit -m "Draft contributing guidelines"
# Push the branch to GitHub
$ git push origin doc/contributing-guidelines
```
Make a pull request/reference issue.

To get the latest changes:

```
# Get the changes from GitHub. Fix any merge conflicts.
$ git pull --rebase origin main
```

To go back to main branch:

```
$ git checkout main
$ git pull --rebase origin main
```
