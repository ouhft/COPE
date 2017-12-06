#!/usr/bin/env bash
# Based on https://ubuntuforums.org/showthread.php?t=2339386
# Remember to **COPY** systemd/system/SystemEmailNotificationStartStop.service, not link it

LAN_IP=`hostname --all-ip-addresses | awk '{print $1}'`
WAN_IP=`wget http://ipinfo.io/ip -qO -`
SERVER_NAME=`hostname -f`
RUNLEVEL=`runlevel`

STARTUP_SUBJECT="[${SERVER_NAME}] - System Startup: "`date`
STARTUP_MESSAGE="$SERVER_NAME @ LAN IP = ${LAN_IP} - WAN IP - ${WAN_IP} Started Successfully at runlevel ${RUNLEVEL}: "`date`
SHUTDOWN_SUBJECT="[${SERVER_NAME}] - System Shutdown: "`date`
SHUTDOWN_MESSAGE="$SERVER_NAME @ LAN IP = ${LAN_IP} - WAN IP - ${WAN_IP} Shutting Down: "`date`
EMAIL_ADDRESS="carl.marshall@nds.ox.ac.uk"
SENDER_ADDRESS="root@dev.nds.ox.ac.uk"
RETVAL=0

stop()
{
   echo  $"Sending Shutdown Email "
   echo "${SHUTDOWN_MESSAGE}" |  mail --set=sendwait --set=from="${SENDER_ADDRESS}" -s "${SHUTDOWN_SUBJECT}" ${EMAIL_ADDRESS}
   #sleep 10
   RETVAL=$?

   return ${RETVAL}
}

start()
{
   echo  $"Sending Startup Email "
   echo "${STARTUP_MESSAGE}" | mail --set=sendwait --set=from="${SENDER_ADDRESS}" -s "${STARTUP_SUBJECT}" ${EMAIL_ADDRESS}
   #sleep 10
   RETVAL=$?

   return ${RETVAL}

}

case "$1" in
start)
   start
;;
stop)
   stop
;;
status)
   echo "Not applied to service"
;;
restart)
   stop
   start
;;
reload)
   echo "Not applied to service"
;;
probe)
;;
*)
   echo "Usage: SystemEmail{start|stop|status|reload|restart}"
   exit 1
;;

esac
exit ${RETVAL}
