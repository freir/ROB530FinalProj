%% Get the DRIFT trajectory

% TODO: record trift trajectory as rosbag
% TODO: figure out initial DRIFT orientation (initial position is (0,0,0))

%% Get the ground truth trajecory from the TartanDrive dataset
bag = rosbag("20210910_3.bag");

odometry = select(bag,'Topic','/odometry/filtered_odom');

msgStructs = readMessages(odometry,'DataFormat','struct');

groundTruthX = zeros(length(msgStructs),1);
groundTruthY = zeros(length(msgStructs),1);
groundTruthZ = zeros(length(msgStructs),1);

initialgroundTruthX = msgStructs{1}.Pose.Pose.Position.X;
initialgroundTruthY = msgStructs{1}.Pose.Pose.Position.Y;
initialgroundTruthZ = msgStructs{1}.Pose.Pose.Position.Z;

transformed_GT_X = zeros(length(msgStructs),1);
transformed_GT_Y = zeros(length(msgStructs),1);
transformed_GT_Z = zeros(length(msgStructs),1);

for msg = 1:length(msgStructs)
    %
    groundTruthX(msg) = msgStructs{msg}.Pose.Pose.Position.X;
    groundTruthY(msg) = msgStructs{msg}.Pose.Pose.Position.Y;
    groundTruthZ(msg) = msgStructs{msg}.Pose.Pose.Position.Z;
    quatx = msgStructs{msg}.Pose.Pose.Orientation.X;
    quaty = msgStructs{msg}.Pose.Pose.Orientation.Y;
    quatz = msgStructs{msg}.Pose.Pose.Orientation.Z;
    quatw = msgStructs{msg}.Pose.Pose.Orientation.W;
    groundTruthOrientQuat(msg) = quaternion(quatw, quatx, quaty, quatz);

    % transform
    transformed_GT_X(msg) = groundTruthX(msg) - initialgroundTruthX;
    transformed_GT_Y(msg) = groundTruthY(msg) - initialgroundTruthY;
    transformed_GT_Z(msg) = groundTruthZ(msg) - initialgroundTruthZ;

end

figure(1)
subplot(1,2,1);
plot3(groundTruthX, groundTruthY, groundTruthZ);
subplot(1,2,2);
plot3(transformed_GT_X, transformed_GT_Y,transformed_GT_Z);