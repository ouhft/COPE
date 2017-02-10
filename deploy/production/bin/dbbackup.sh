#!/usr/bin/env bash

SOURCE=/sites/py3_cope/cope_repo/db.sqlite3
TARGET=/sites/py3_cope/db-backup/`date "+%Y-%m-%d"`.sqlite3.tar.gz

echo "Starting backup as `whoami`"
tar -czf ${TARGET} ${SOURCE}
echo "DB Backup done from $SOURCE to $TARGET"