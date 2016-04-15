#!/usr/bin/env bash

TARGET=/sites/cope/db-backup/`date "+%Y-%m-%d"`.sqlite3
SOURCE=/sites/cope/cope_repo/db.sqlite3

cp SOURCE TARGET
echo "DB Backup done to $TARGET"