% Performs a rigid body transformation for the output of DRIFT
% DRIFT always initializes at (0,0,0) with the vehicle pointed in the +x
% direction. Drift outputs a 8 column space delimeted table with columns
% time x y z qx qy qz qw.
% Use the values of x y z qx qy qz qw from the corresponding timestamp in
% the ground truth file to set the inputs.
% Set the desired output file name in the last row

%% Inputs
x0 = 315.18;
y0 = -938.52;
z0 = -59.96;

quatx = 0.0084;
quaty = 0.0255;
quatz = -0.2592;
quatw = 0.9655;

input_file = 'input_file.txt'; % DRIFT output
output_file = 'output_file_name.txt'; % Will write transformed DRIFT output

%% Calculations
quat = quaternion(quatw,quatx,quaty,quatz);
axang = quat2axang(quat);
axang(4) = axang(4) - pi/2; % by inspection the trajectory needed an additional
% 90 degree rotation about the axis of rotation, which was basically +z
quat = axang2quat(axang);

drift = load(input_file);

rotm = quat2rotm(quat);
trans = [x0; y0; z0];
RBT = [rotm trans; 0 0 0 1];

transformed_drift = drift;

rows = size(drift, 1);

for row = 1:rows
    x = drift(row, 2);
    y = drift(row, 3);
    z = drift(row, 4);
    homogeneous_pt = [x;y;z;1];
    transformed_pt = RBT*homogeneous_pt; % 4x1

    transformed_drift(row, 2) = transformed_pt(1);
    transformed_drift(row, 3) = transformed_pt(2);
    transformed_drift(row, 4) = transformed_pt(3);

end
%% Write output to space delimited text file
writematrix(transformed_drift, output_file, 'Delimiter',' ');