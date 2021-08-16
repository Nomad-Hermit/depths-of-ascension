using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeedManager 
{
    //****************************************************************************************|
    //                                                                                        |
    // Manages the feed of information of the game.                                           |
    //                                                                                        |
    //****************************************************************************************|

    static public LocalizationData localization;

    // Adds a line to the feed when the player attacks.
    static public void PlayerAttack(string enemy, bool hit) {
        Text feed = GameObject.Find("FeedText").GetComponent<Text>();

        feed.text = feed.text + "\n" + localization.GetString("feed_plattack_txt") + enemy;

        if (hit) {
            feed.text = feed.text + localization.GetString("feed_hit_txt");
        } else {
            feed.text = feed.text + localization.GetString("feed_missed_txt");
        }
    }

    // Adds a line to the feed when an enemy attacks.
    static public void EnemyAttacks(string enemy, bool hit) {
        Text feed = GameObject.Find("FeedText").GetComponent<Text>();

        feed.text = feed.text + "\n" + enemy + localization.GetString("feed_enattack_txt");

        if (hit) {
            feed.text = feed.text + localization.GetString("feed_hit_txt");
        }
        else {
            feed.text = feed.text + localization.GetString("feed_missed_txt");
        }
    }

    // Adds a line to the feed when the player receives damage.
    static public void DamagePlayer(int damage) {
        Text feed = GameObject.Find("FeedText").GetComponent<Text>();

        feed.text = feed.text + "\n" + localization.GetString("feed_pldamage_txt") + damage.ToString() + localization.GetString("feed_damage_txt");
    }

    // Adds a line to the feed when an enemy receives damage.
    static public void DamageEnemy(string enemy, int damage) {
        Text feed = GameObject.Find("FeedText").GetComponent<Text>();

        feed.text = feed.text + "\n" + enemy + localization.GetString("feed_endamage_txt") + damage.ToString() + localization.GetString("feed_damage_txt");
    }

    // Adds a line to the feed when an enemy dies.
    static public void EnemyDied(string enemy) {
        Text feed = GameObject.Find("FeedText").GetComponent<Text>();

        feed.text = feed.text + "\n" + enemy + localization.GetString("feed_die_txt");
    }

    // Adds a line to the feed when the player levels up.
    static public void LevelUp() {
        Text feed = GameObject.Find("FeedText").GetComponent<Text>();

        feed.text = feed.text + "\n" + localization.GetString("feed_levelup_txt");
    }
}
