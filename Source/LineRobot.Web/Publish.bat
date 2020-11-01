call dotnet publish -c Release -r linux-x64

call docker build -t atkseegow/line_robot_web:1.0.0 .
call docker push atkseegow/line_robot_web:1.0.0

call docker tag atkseegow/line_robot_web:1.0.0 atkseegow/line_robot_web:latest 
call docker push atkseegow/line_robot_web:latest
