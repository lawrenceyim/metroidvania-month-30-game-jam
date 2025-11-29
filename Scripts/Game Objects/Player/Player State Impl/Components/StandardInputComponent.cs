using InputSystem;

namespace PlayerSystem;

public class StandardInputComponent {
    public static void Input(InputEventDto dto, Player player) {
        if (dto is KeyDto keyDto) {
            player.SetKeyPressed(keyDto.Identifier, keyDto.Pressed);

            if (keyDto.Identifier == "Space" && keyDto.Pressed) {
                player.SwitchState(PlayerStateId.Jumping);
            }
        }
    }
}