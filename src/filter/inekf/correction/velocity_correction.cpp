#include "filter/inekf/correction/velocity_correction.h"

namespace inekf {
using namespace std;
using namespace lie_group;

VelocityCorrection::VelocityCorrection(VelocityQueuePtr sensor_data_buffer_ptr,
                                       const ErrorType& error_type,
                                       const Eigen::Matrix3d& covariance)
    : Correction::Correction(),
      sensor_data_buffer_ptr_(sensor_data_buffer_ptr),
      sensor_data_buffer_(*sensor_data_buffer_ptr.get()),
      error_type_(error_type),
      covariance_(covariance) {
  correction_type_ = CorrectionType::VELOCITY;
}

const VelocityQueuePtr VelocityCorrection::get_sensor_data_buffer_ptr() const {
  return sensor_data_buffer_ptr_;
}

// Correct using measured body velocity with the estimated velocity
void VelocityCorrection::Correct(RobotState& state) {
  Eigen::VectorXd Z, Y, b;
  Eigen::MatrixXd H, N, PI;

  // Fill out observation data
  int dimX = state.dimX();
  int dimTheta = state.dimTheta();
  int dimP = state.dimP();

  // Get latest measurement:
  const Eigen::Vector3d measured_velocity
      = sensor_data_buffer_.front().get()->get_velocity();
  sensor_data_buffer_.pop();

  // Fill out Y
  // Y.conservativeResize(dimX, Eigen::NoChange);
  // Y.segment(0,dimX) = Eigen::VectorXd::Zero(dimX);
  // Y.segment<3>(0) = measured_velocity;
  // Y(3) = -1;

  // // Fill out b
  // b.conservativeResize(dimX, Eigen::NoChange);
  // b.segment(0,dimX) = Eigen::VectorXd::Zero(dimX);
  // b(3) = -1;

  // Fill out H
  H.conservativeResize(3, dimP);
  H.block(0, 0, 3, dimP) = Eigen::MatrixXd::Zero(3, dimP);
  H.block(0, 3, 3, 3) = Eigen::Matrix3d::Identity();

  // Fill out N
  N.conservativeResize(3, 3);
  N = covariance_;

  // Fill out PI
  // PI.conservativeResize(3, dimX);
  // PI.block(0,0,3,dimX) = Eigen::MatrixXd::Zero(3,dimX);
  // PI.block(0,0,3,3) = Eigen::Matrix3d::Identity();

  // Fill out Z
  // Z = X*Y-b = PI*X*Y
  Eigen::Matrix3d R = state.get_rotation();
  Eigen::Vector3d v = state.get_velocity();


  int startIndex = Z.rows();
  Z.conservativeResize(startIndex + 3, Eigen::NoChange);
  Z.segment(0, 3) = R * measured_velocity - v;

  // Correct state using stacked observation
  if (Z.rows() > 0) {
    CorrectRightInvariant(Z, H, N, state, error_type_);
  }
}
}    // namespace inekf