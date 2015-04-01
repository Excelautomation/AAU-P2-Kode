#!/bin/sh
 
git filter-branch -f --env-filter '

OLD_EMAIL="WIN-BFM6JJ47JRD\Martin"
CORRECT_NAME="Myrionsc2"
CORRECT_EMAIL="marpht@hotmail.com"
 
if [ "$GIT_COMMITTER_EMAIL" = "$OLD_EMAIL" ]
then
    export GIT_COMMITTER_NAME="$CORRECT_NAME"
    export GIT_COMMITTER_EMAIL="$CORRECT_EMAIL"
fi
if [ "$GIT_AUTHOR_EMAIL" = "$OLD_EMAIL" ]
then
    export GIT_AUTHOR_NAME="$CORRECT_NAME"
    export GIT_AUTHOR_EMAIL="$CORRECT_EMAIL"
fi
' --tag-name-filter cat -- --branches --tags