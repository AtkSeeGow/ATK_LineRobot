#!/bin/bash

docker stop line_robot_web
docker rm line_robot_web

echo y | docker system prune -a

docker run -d -v /home/atkseegow/certificate:/certificate -p 8915:5000 -e "TZ=Asia/Taipei" -p 8916:5001 --restart=always --link=mongodb --hostname line_robot_web --name line_robot_web atkseegow/line_robot_web

docker cp /home/atkseegow/line_robot_web/appsettings.json line_robot_web:/app/appsettings.json
docker cp /home/atkseegow/line_robot_web/NLog.config line_robot_web:/app/NLog.config

docker restart line_robot_web

exit;
EOF