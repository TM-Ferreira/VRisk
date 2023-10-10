using UnityEngine;

public class TimerDisplay : MonoBehaviour
{
    public ClockCharacter seconds_ones_char;
    public ClockCharacter seconds_tens_char;
    public ClockCharacter minutes_ones_char;
    public ClockCharacter minutes_tens_char;

    private int seconds;
    private int minutes;
    
    public void updateTimer(float _time_in_secs)
    {
        int seconds_since_start = Mathf.FloorToInt(_time_in_secs);

        seconds = seconds_since_start % 60;
        minutes = seconds_since_start / 60;

        updateDisplay();
    }

    public void resetTimer()
    {
        seconds = 0;
        minutes = 0;
        
        updateDisplay();
    }

    private void updateDisplay()
    {
        ClockCharacter.Character secondsOnes = (ClockCharacter.Character)(seconds % 10);
        ClockCharacter.Character secondsTens = (ClockCharacter.Character)(seconds / 10);

        ClockCharacter.Character minutesOnes = (ClockCharacter.Character)(minutes % 10);
        ClockCharacter.Character minutesTens = (ClockCharacter.Character)(minutes / 10);

        seconds_ones_char.setCharacter(secondsOnes);
        seconds_tens_char.setCharacter(secondsTens);

        minutes_ones_char.setCharacter(minutesOnes);
        minutes_tens_char.setCharacter(minutesTens);
    }
}
