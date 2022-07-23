# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [v0.0.0] - 2022-07-23
### Added
- Component and prefab **GravityDirectionSystem** (ECS system) to control falling 
  for custom **ThirdPersonController** objects.
- On-falling stuck check tests for **GravityDirectionSystem**.
- Custom **CharacterController** to allow rotate CapsuleCollider with character.
- **CharacterController::Move(Vector3)** method with draft collision resolver.

### Changed
- **ThirdPersonController** use custom **CharacterController**.

### Removed
- **GravityDirectionEntity** component.


## [v0.0.0] - 2022-07-22
### Added
- **ThirdPersonController** component to makes custom is-grounded detection.
- Runtime tests for **ThirdPersonController** is-grounded flag calculation.
