# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [v0.0.0] - 2022-07-29
### Changed 
- Collision detection - try binary search for distance movement correction. 
  Better wall detection but character stops when should move along wall. Required an improvement.
- Test case **CharacterControllerTests_Move::IsNotMoveThroughWalls** - 
  use manual data-rows to reproduce more cases of movement to walls.

## [v0.0.0] - 2022-07-24
### Added
- Input actions and inputs reader

### Changed 
- Component **GravityDirectionSystem** controls **CharacterController**,
  because this kind calculation more related to physics, not to character controlling with player.


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
