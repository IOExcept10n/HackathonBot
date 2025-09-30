using MyBots.Core.Fsm.States;

namespace MyBots.Modules.Common.Interactivity;

public static class EmojiExtensions
{
    private const string UnknownEmojiSign = "?";

    public static Emoji AsEmoji(this bool v) => v ? Emoji.WhiteHeavyCheckMark : Emoji.CrossMark;

    public static string ToUnicode(this Emoji emoji) => emoji switch
    {
        Emoji.None => string.Empty,
        Emoji.GrinningFaceWithSmilingEyes => EmojiStrings.GRINNING_FACE_WITH_SMILING_EYES,
        Emoji.FaceWithTearsOfJoy => EmojiStrings.FACE_WITH_TEARS_OF_JOY,
        Emoji.SmilingFaceWithOpenMouth => EmojiStrings.SMILING_FACE_WITH_OPEN_MOUTH,
        Emoji.SmilingFaceWithOpenMouthAndSmilingEyes => EmojiStrings.SMILING_FACE_WITH_OPEN_MOUTH_AND_SMILING_EYES,
        Emoji.SmilingFaceWithOpenMouthAndColdSweat => EmojiStrings.SMILING_FACE_WITH_OPEN_MOUTH_AND_COLD_SWEAT,
        Emoji.SmilingFaceWithOpenMouthAndTightlyClosedEyes => EmojiStrings.SMILING_FACE_WITH_OPEN_MOUTH_AND_TIGHTLY_CLOSED_EYES,
        Emoji.WinkingFace => EmojiStrings.WINKING_FACE,
        Emoji.SmilingFaceWithSmilingEyes => EmojiStrings.SMILING_FACE_WITH_SMILING_EYES,
        Emoji.FaceSavouringDeliciousFood => EmojiStrings.FACE_SAVOURING_DELICIOUS_FOOD,
        Emoji.RelievedFace => EmojiStrings.RELIEVED_FACE,
        Emoji.SmilingFaceWithHeartShapedEyes => EmojiStrings.SMILING_FACE_WITH_HEART_SHAPED_EYES,
        Emoji.SmirkingFace => EmojiStrings.SMIRKING_FACE,
        Emoji.UnamusedFace => EmojiStrings.UNAMUSED_FACE,
        Emoji.FaceWithColdSweat => EmojiStrings.FACE_WITH_COLD_SWEAT,
        Emoji.PensiveFace => EmojiStrings.PENSIVE_FACE,
        Emoji.ConfoundedFace => EmojiStrings.CONFOUNDED_FACE,
        Emoji.FaceThrowingAKiss => EmojiStrings.FACE_THROWING_A_KISS,
        Emoji.KissingFaceWithClosedEyes => EmojiStrings.KISSING_FACE_WITH_CLOSED_EYES,
        Emoji.FaceWithStuckOutTongueAndWinkingEye => EmojiStrings.FACE_WITH_STUCK_OUT_TONGUE_AND_WINKING_EYE,
        Emoji.FaceWithStuckOutTongueAndTightlyClosedEyes => EmojiStrings.FACE_WITH_STUCK_OUT_TONGUE_AND_TIGHTLY_CLOSED_EYES,
        Emoji.DisappointedFace => EmojiStrings.DISAPPOINTED_FACE,
        Emoji.AngryFace => EmojiStrings.ANGRY_FACE,
        Emoji.PoutingFace => EmojiStrings.POUTING_FACE,
        Emoji.CryingFace => EmojiStrings.CRYING_FACE,
        Emoji.PerseveringFace => EmojiStrings.PERSEVERING_FACE,
        Emoji.FaceWithLookOfTriumph => EmojiStrings.FACE_WITH_LOOK_OF_TRIUMPH,
        Emoji.DisappointedButRelievedFace => EmojiStrings.DISAPPOINTED_BUT_RELIEVED_FACE,
        Emoji.FearfulFace => EmojiStrings.FEARFUL_FACE,
        Emoji.WearyFace => EmojiStrings.WEARY_FACE,
        Emoji.SleepyFace => EmojiStrings.SLEEPY_FACE,
        Emoji.TiredFace => EmojiStrings.TIRED_FACE,
        Emoji.LoudlyCryingFace => EmojiStrings.LOUDLY_CRYING_FACE,
        Emoji.FaceWithOpenMouthAndColdSweat => EmojiStrings.FACE_WITH_OPEN_MOUTH_AND_COLD_SWEAT,
        Emoji.FaceScreamingInFear => EmojiStrings.FACE_SCREAMING_IN_FEAR,
        Emoji.AstonishedFace => EmojiStrings.ASTONISHED_FACE,
        Emoji.FlushedFace => EmojiStrings.FLUSHED_FACE,
        Emoji.DizzyFace => EmojiStrings.DIZZY_FACE,
        Emoji.FaceWithMedicalMask => EmojiStrings.FACE_WITH_MEDICAL_MASK,
        Emoji.GrinningCatFaceWithSmilingEyes => EmojiStrings.GRINNING_CAT_FACE_WITH_SMILING_EYES,
        Emoji.CatFaceWithTearsOfJoy => EmojiStrings.CAT_FACE_WITH_TEARS_OF_JOY,
        Emoji.SmilingCatFaceWithOpenMouth => EmojiStrings.SMILING_CAT_FACE_WITH_OPEN_MOUTH,
        Emoji.SmilingCatFaceWithHeartShapedEyes => EmojiStrings.SMILING_CAT_FACE_WITH_HEART_SHAPED_EYES,
        Emoji.CatFaceWithWrySmile => EmojiStrings.CAT_FACE_WITH_WRY_SMILE,
        Emoji.KissingCatFaceWithClosedEyes => EmojiStrings.KISSING_CAT_FACE_WITH_CLOSED_EYES,
        Emoji.PoutingCatFace => EmojiStrings.POUTING_CAT_FACE,
        Emoji.CryingCatFace => EmojiStrings.CRYING_CAT_FACE,
        Emoji.WearyCatFace => EmojiStrings.WEARY_CAT_FACE,
        Emoji.FaceWithNoGoodGesture => EmojiStrings.FACE_WITH_NO_GOOD_GESTURE,
        Emoji.FaceWithOkGesture => EmojiStrings.FACE_WITH_OK_GESTURE,
        Emoji.PersonBowingDeeply => EmojiStrings.PERSON_BOWING_DEEPLY,
        Emoji.SeeNoEvilMonkey => EmojiStrings.SEE_NO_EVIL_MONKEY,
        Emoji.HearNoEvilMonkey => EmojiStrings.HEAR_NO_EVIL_MONKEY,
        Emoji.SpeakNoEvilMonkey => EmojiStrings.SPEAK_NO_EVIL_MONKEY,
        Emoji.HappyPersonRaisingOneHand => EmojiStrings.HAPPY_PERSON_RAISING_ONE_HAND,
        Emoji.PersonRaisingBothHandsInCelebration => EmojiStrings.PERSON_RAISING_BOTH_HANDS_IN_CELEBRATION,
        Emoji.PersonFrowning => EmojiStrings.PERSON_FROWNING,
        Emoji.PersonWithPoutingFace => EmojiStrings.PERSON_WITH_POUTING_FACE,
        Emoji.PersonWithFoldedHands => EmojiStrings.PERSON_WITH_FOLDED_HANDS,
        Emoji.BlackScissors => EmojiStrings.BLACK_SCISSORS,
        Emoji.WhiteHeavyCheckMark => EmojiStrings.WHITE_HEAVY_CHECK_MARK,
        Emoji.Airplane => EmojiStrings.AIRPLANE,
        Emoji.Envelope => EmojiStrings.ENVELOPE,
        Emoji.RaisedFist => EmojiStrings.RAISED_FIST,
        Emoji.RaisedHand => EmojiStrings.RAISED_HAND,
        Emoji.VictoryHand => EmojiStrings.VICTORY_HAND,
        Emoji.Pencil => EmojiStrings.PENCIL,
        Emoji.BlackNib => EmojiStrings.BLACK_NIB,
        Emoji.HeavyCheckMark => EmojiStrings.HEAVY_CHECK_MARK,
        Emoji.HeavyMultiplicationX => EmojiStrings.HEAVY_MULTIPLICATION_X,
        Emoji.Sparkles => EmojiStrings.SPARKLES,
        Emoji.EightSpokedAsterisk => EmojiStrings.EIGHT_SPOKED_ASTERISK,
        Emoji.EightPointedBlackStar => EmojiStrings.EIGHT_POINTED_BLACK_STAR,
        Emoji.Snowflake => EmojiStrings.SNOWFLAKE,
        Emoji.Sparkle => EmojiStrings.SPARKLE,
        Emoji.CrossMark => EmojiStrings.CROSS_MARK,
        Emoji.NegativeSquaredCrossMark => EmojiStrings.NEGATIVE_SQUARED_CROSS_MARK,
        Emoji.BlackQuestionMarkOrnament => EmojiStrings.BLACK_QUESTION_MARK_ORNAMENT,
        Emoji.WhiteQuestionMarkOrnament => EmojiStrings.WHITE_QUESTION_MARK_ORNAMENT,
        Emoji.WhiteExclamationMarkOrnament => EmojiStrings.WHITE_EXCLAMATION_MARK_ORNAMENT,
        Emoji.HeavyExclamationMarkSymbol => EmojiStrings.HEAVY_EXCLAMATION_MARK_SYMBOL,
        Emoji.HeavyBlackHeart => EmojiStrings.HEAVY_BLACK_HEART,
        Emoji.HeavyPlusSign => EmojiStrings.HEAVY_PLUS_SIGN,
        Emoji.HeavyMinusSign => EmojiStrings.HEAVY_MINUS_SIGN,
        Emoji.HeavyDivisionSign => EmojiStrings.HEAVY_DIVISION_SIGN,
        Emoji.BlackRightwardsArrow => EmojiStrings.BLACK_RIGHTWARDS_ARROW,
        Emoji.CurlyLoop => EmojiStrings.CURLY_LOOP,
        Emoji.Rocket => EmojiStrings.ROCKET,
        Emoji.RailwayCar => EmojiStrings.RAILWAY_CAR,
        Emoji.HighSpeedTrain => EmojiStrings.HIGH_SPEED_TRAIN,
        Emoji.HighSpeedTrainWithBulletNose => EmojiStrings.HIGH_SPEED_TRAIN_WITH_BULLET_NOSE,
        Emoji.Metro => EmojiStrings.METRO,
        Emoji.Station => EmojiStrings.STATION,
        Emoji.Bus => EmojiStrings.BUS,
        Emoji.BusStop => EmojiStrings.BUS_STOP,
        Emoji.Ambulance => EmojiStrings.AMBULANCE,
        Emoji.FireEngine => EmojiStrings.FIRE_ENGINE,
        Emoji.PoliceCar => EmojiStrings.POLICE_CAR,
        Emoji.Taxi => EmojiStrings.TAXI,
        Emoji.Automobile => EmojiStrings.AUTOMOBILE,
        Emoji.RecreationalVehicle => EmojiStrings.RECREATIONAL_VEHICLE,
        Emoji.DeliveryTruck => EmojiStrings.DELIVERY_TRUCK,
        Emoji.Ship => EmojiStrings.SHIP,
        Emoji.Speedboat => EmojiStrings.SPEEDBOAT,
        Emoji.HorizontalTrafficLight => EmojiStrings.HORIZONTAL_TRAFFIC_LIGHT,
        Emoji.ConstructionSign => EmojiStrings.CONSTRUCTION_SIGN,
        Emoji.PoliceCarsRevolvingLight => EmojiStrings.POLICE_CARS_REVOLVING_LIGHT,
        Emoji.TriangularFlagOnPost => EmojiStrings.TRIANGULAR_FLAG_ON_POST,
        Emoji.Door => EmojiStrings.DOOR,
        Emoji.NoEntrySign => EmojiStrings.NO_ENTRY_SIGN,
        Emoji.SmokingSymbol => EmojiStrings.SMOKING_SYMBOL,
        Emoji.NoSmokingSymbol => EmojiStrings.NO_SMOKING_SYMBOL,
        Emoji.Bicycle => EmojiStrings.BICYCLE,
        Emoji.Pedestrian => EmojiStrings.PEDESTRIAN,
        Emoji.MensSymbol => EmojiStrings.MENS_SYMBOL,
        Emoji.WomensSymbol => EmojiStrings.WOMENS_SYMBOL,
        Emoji.Restroom => EmojiStrings.RESTROOM,
        Emoji.BabySymbol => EmojiStrings.BABY_SYMBOL,
        Emoji.Toilet => EmojiStrings.TOILET,
        Emoji.WaterCloset => EmojiStrings.WATER_CLOSET,
        Emoji.Bath => EmojiStrings.BATH,
        Emoji.CircledLatinCapitalLetterM => EmojiStrings.CIRCLED_LATIN_CAPITAL_LETTER_M,
        Emoji.NegativeSquaredLatinCapitalLetterA => EmojiStrings.NEGATIVE_SQUARED_LATIN_CAPITAL_LETTER_A,
        Emoji.NegativeSquaredLatinCapitalLetterB => EmojiStrings.NEGATIVE_SQUARED_LATIN_CAPITAL_LETTER_B,
        Emoji.NegativeSquaredLatinCapitalLetterO => EmojiStrings.NEGATIVE_SQUARED_LATIN_CAPITAL_LETTER_O,
        Emoji.NegativeSquaredLatinCapitalLetterP => EmojiStrings.NEGATIVE_SQUARED_LATIN_CAPITAL_LETTER_P,
        Emoji.NegativeSquaredAb => EmojiStrings.NEGATIVE_SQUARED_AB,
        Emoji.SquaredCl => EmojiStrings.SQUARED_CL,
        Emoji.SquaredCool => EmojiStrings.SQUARED_COOL,
        Emoji.SquaredFree => EmojiStrings.SQUARED_FREE,
        Emoji.SquaredId => EmojiStrings.SQUARED_ID,
        Emoji.SquaredNew => EmojiStrings.SQUARED_NEW,
        Emoji.SquaredNg => EmojiStrings.SQUARED_NG,
        Emoji.SquaredOk => EmojiStrings.SQUARED_OK,
        Emoji.SquaredSos => EmojiStrings.SQUARED_SOS,
        Emoji.SquaredUpWithExclamationMark => EmojiStrings.SQUARED_UP_WITH_EXCLAMATION_MARK,
        Emoji.SquaredVs => EmojiStrings.SQUARED_VS,
        Emoji.RegionalIndicatorSymbolLetterDRegionalIndicatorSymbolLetterE => EmojiStrings.REGIONAL_INDICATOR_SYMBOL_LETTER_D_REGIONAL_INDICATOR_SYMBOL_LETTER_E,
        Emoji.RegionalIndicatorSymbolLetterGRegionalIndicatorSymbolLetterB => EmojiStrings.REGIONAL_INDICATOR_SYMBOL_LETTER_G_REGIONAL_INDICATOR_SYMBOL_LETTER_B,
        Emoji.RegionalIndicatorSymbolLetterCRegionalIndicatorSymbolLetterN => EmojiStrings.REGIONAL_INDICATOR_SYMBOL_LETTER_C_REGIONAL_INDICATOR_SYMBOL_LETTER_N,
        Emoji.RegionalIndicatorSymbolLetterJRegionalIndicatorSymbolLetterP => EmojiStrings.REGIONAL_INDICATOR_SYMBOL_LETTER_J_REGIONAL_INDICATOR_SYMBOL_LETTER_P,
        Emoji.RegionalIndicatorSymbolLetterFRegionalIndicatorSymbolLetterR => EmojiStrings.REGIONAL_INDICATOR_SYMBOL_LETTER_F_REGIONAL_INDICATOR_SYMBOL_LETTER_R,
        Emoji.RegionalIndicatorSymbolLetterKRegionalIndicatorSymbolLetterR => EmojiStrings.REGIONAL_INDICATOR_SYMBOL_LETTER_K_REGIONAL_INDICATOR_SYMBOL_LETTER_R,
        Emoji.RegionalIndicatorSymbolLetterERegionalIndicatorSymbolLetterS => EmojiStrings.REGIONAL_INDICATOR_SYMBOL_LETTER_E_REGIONAL_INDICATOR_SYMBOL_LETTER_S,
        Emoji.RegionalIndicatorSymbolLetterIRegionalIndicatorSymbolLetterT => EmojiStrings.REGIONAL_INDICATOR_SYMBOL_LETTER_I_REGIONAL_INDICATOR_SYMBOL_LETTER_T,
        Emoji.RegionalIndicatorSymbolLetterRRegionalIndicatorSymbolLetterU => EmojiStrings.REGIONAL_INDICATOR_SYMBOL_LETTER_R_REGIONAL_INDICATOR_SYMBOL_LETTER_U,
        Emoji.RegionalIndicatorSymbolLetterURegionalIndicatorSymbolLetterS => EmojiStrings.REGIONAL_INDICATOR_SYMBOL_LETTER_U_REGIONAL_INDICATOR_SYMBOL_LETTER_S,
        Emoji.SquaredKatakanaKoko => EmojiStrings.SQUARED_KATAKANA_KOKO,
        Emoji.SquaredKatakanaSa => EmojiStrings.SQUARED_KATAKANA_SA,
        Emoji.SquaredCjkUnifiedIdeograph7121 => EmojiStrings.SQUARED_CJK_UNIFIED_IDEOGRAPH_7121,
        Emoji.SquaredCjkUnifiedIdeograph6307 => EmojiStrings.SQUARED_CJK_UNIFIED_IDEOGRAPH_6307,
        Emoji.SquaredCjkUnifiedIdeograph7981 => EmojiStrings.SQUARED_CJK_UNIFIED_IDEOGRAPH_7981,
        Emoji.SquaredCjkUnifiedIdeograph7a7a => EmojiStrings.SQUARED_CJK_UNIFIED_IDEOGRAPH_7A7A,
        Emoji.SquaredCjkUnifiedIdeograph5408 => EmojiStrings.SQUARED_CJK_UNIFIED_IDEOGRAPH_5408,
        Emoji.SquaredCjkUnifiedIdeograph6e80 => EmojiStrings.SQUARED_CJK_UNIFIED_IDEOGRAPH_6E80,
        Emoji.SquaredCjkUnifiedIdeograph6709 => EmojiStrings.SQUARED_CJK_UNIFIED_IDEOGRAPH_6709,
        Emoji.SquaredCjkUnifiedIdeograph6708 => EmojiStrings.SQUARED_CJK_UNIFIED_IDEOGRAPH_6708,
        Emoji.SquaredCjkUnifiedIdeograph7533 => EmojiStrings.SQUARED_CJK_UNIFIED_IDEOGRAPH_7533,
        Emoji.SquaredCjkUnifiedIdeograph5272 => EmojiStrings.SQUARED_CJK_UNIFIED_IDEOGRAPH_5272,
        Emoji.SquaredCjkUnifiedIdeograph55b6 => EmojiStrings.SQUARED_CJK_UNIFIED_IDEOGRAPH_55B6,
        Emoji.CircledIdeographAdvantage => EmojiStrings.CIRCLED_IDEOGRAPH_ADVANTAGE,
        Emoji.CircledIdeographAccept => EmojiStrings.CIRCLED_IDEOGRAPH_ACCEPT,
        Emoji.CopyrightSign => EmojiStrings.COPYRIGHT_SIGN,
        Emoji.RegisteredSign => EmojiStrings.REGISTERED_SIGN,
        Emoji.DoubleExclamationMark => EmojiStrings.DOUBLE_EXCLAMATION_MARK,
        Emoji.ExclamationQuestionMark => EmojiStrings.EXCLAMATION_QUESTION_MARK,
        Emoji.NumberSignCombiningEnclosingKeycap => EmojiStrings.NUMBER_SIGN_COMBINING_ENCLOSING_KEYCAP,
        Emoji.DigitEightCombiningEnclosingKeycap => EmojiStrings.DIGIT_EIGHT_COMBINING_ENCLOSING_KEYCAP,
        Emoji.DigitNineCombiningEnclosingKeycap => EmojiStrings.DIGIT_NINE_COMBINING_ENCLOSING_KEYCAP,
        Emoji.DigitSevenCombiningEnclosingKeycap => EmojiStrings.DIGIT_SEVEN_COMBINING_ENCLOSING_KEYCAP,
        Emoji.DigitZeroCombiningEnclosingKeycap => EmojiStrings.DIGIT_ZERO_COMBINING_ENCLOSING_KEYCAP,
        Emoji.DigitSixCombiningEnclosingKeycap => EmojiStrings.DIGIT_SIX_COMBINING_ENCLOSING_KEYCAP,
        Emoji.DigitFiveCombiningEnclosingKeycap => EmojiStrings.DIGIT_FIVE_COMBINING_ENCLOSING_KEYCAP,
        Emoji.DigitFourCombiningEnclosingKeycap => EmojiStrings.DIGIT_FOUR_COMBINING_ENCLOSING_KEYCAP,
        Emoji.DigitThreeCombiningEnclosingKeycap => EmojiStrings.DIGIT_THREE_COMBINING_ENCLOSING_KEYCAP,
        Emoji.DigitTwoCombiningEnclosingKeycap => EmojiStrings.DIGIT_TWO_COMBINING_ENCLOSING_KEYCAP,
        Emoji.DigitOneCombiningEnclosingKeycap => EmojiStrings.DIGIT_ONE_COMBINING_ENCLOSING_KEYCAP,
        Emoji.TradeMarkSign => EmojiStrings.TRADE_MARK_SIGN,
        Emoji.InformationSource => EmojiStrings.INFORMATION_SOURCE,
        Emoji.LeftRightArrow => EmojiStrings.LEFT_RIGHT_ARROW,
        Emoji.UpDownArrow => EmojiStrings.UP_DOWN_ARROW,
        Emoji.NorthWestArrow => EmojiStrings.NORTH_WEST_ARROW,
        Emoji.NorthEastArrow => EmojiStrings.NORTH_EAST_ARROW,
        Emoji.SouthEastArrow => EmojiStrings.SOUTH_EAST_ARROW,
        Emoji.SouthWestArrow => EmojiStrings.SOUTH_WEST_ARROW,
        Emoji.LeftwardsArrowWithHook => EmojiStrings.LEFTWARDS_ARROW_WITH_HOOK,
        Emoji.RightwardsArrowWithHook => EmojiStrings.RIGHTWARDS_ARROW_WITH_HOOK,
        Emoji.Watch => EmojiStrings.WATCH,
        Emoji.Hourglass => EmojiStrings.HOURGLASS,
        Emoji.BlackRightPointingDoubleTriangle => EmojiStrings.BLACK_RIGHT_POINTING_DOUBLE_TRIANGLE,
        Emoji.BlackLeftPointingDoubleTriangle => EmojiStrings.BLACK_LEFT_POINTING_DOUBLE_TRIANGLE,
        Emoji.BlackUpPointingDoubleTriangle => EmojiStrings.BLACK_UP_POINTING_DOUBLE_TRIANGLE,
        Emoji.BlackDownPointingDoubleTriangle => EmojiStrings.BLACK_DOWN_POINTING_DOUBLE_TRIANGLE,
        Emoji.AlarmClock => EmojiStrings.ALARM_CLOCK,
        Emoji.HourglassWithFlowingSand => EmojiStrings.HOURGLASS_WITH_FLOWING_SAND,
        Emoji.BlackSmallSquare => EmojiStrings.BLACK_SMALL_SQUARE,
        Emoji.WhiteSmallSquare => EmojiStrings.WHITE_SMALL_SQUARE,
        Emoji.BlackRightPointingTriangle => EmojiStrings.BLACK_RIGHT_POINTING_TRIANGLE,
        Emoji.BlackLeftPointingTriangle => EmojiStrings.BLACK_LEFT_POINTING_TRIANGLE,
        Emoji.WhiteMediumSquare => EmojiStrings.WHITE_MEDIUM_SQUARE,
        Emoji.BlackMediumSquare => EmojiStrings.BLACK_MEDIUM_SQUARE,
        Emoji.WhiteMediumSmallSquare => EmojiStrings.WHITE_MEDIUM_SMALL_SQUARE,
        Emoji.BlackMediumSmallSquare => EmojiStrings.BLACK_MEDIUM_SMALL_SQUARE,
        Emoji.BlackSunWithRays => EmojiStrings.BLACK_SUN_WITH_RAYS,
        Emoji.Cloud => EmojiStrings.CLOUD,
        Emoji.BlackTelephone => EmojiStrings.BLACK_TELEPHONE,
        Emoji.BallotBoxWithCheck => EmojiStrings.BALLOT_BOX_WITH_CHECK,
        Emoji.UmbrellaWithRainDrops => EmojiStrings.UMBRELLA_WITH_RAIN_DROPS,
        Emoji.HotBeverage => EmojiStrings.HOT_BEVERAGE,
        Emoji.WhiteUpPointingIndex => EmojiStrings.WHITE_UP_POINTING_INDEX,
        Emoji.WhiteSmilingFace => EmojiStrings.WHITE_SMILING_FACE,
        Emoji.Aries => EmojiStrings.ARIES,
        Emoji.Taurus => EmojiStrings.TAURUS,
        Emoji.Gemini => EmojiStrings.GEMINI,
        Emoji.Cancer => EmojiStrings.CANCER,
        Emoji.Leo => EmojiStrings.LEO,
        Emoji.Virgo => EmojiStrings.VIRGO,
        Emoji.Libra => EmojiStrings.LIBRA,
        Emoji.Scorpius => EmojiStrings.SCORPIUS,
        Emoji.Sagittarius => EmojiStrings.SAGITTARIUS,
        Emoji.Capricorn => EmojiStrings.CAPRICORN,
        Emoji.Aquarius => EmojiStrings.AQUARIUS,
        Emoji.Pisces => EmojiStrings.PISCES,
        Emoji.BlackSpadeSuit => EmojiStrings.BLACK_SPADE_SUIT,
        Emoji.BlackClubSuit => EmojiStrings.BLACK_CLUB_SUIT,
        Emoji.BlackHeartSuit => EmojiStrings.BLACK_HEART_SUIT,
        Emoji.BlackDiamondSuit => EmojiStrings.BLACK_DIAMOND_SUIT,
        Emoji.HotSprings => EmojiStrings.HOT_SPRINGS,
        Emoji.BlackUniversalRecyclingSymbol => EmojiStrings.BLACK_UNIVERSAL_RECYCLING_SYMBOL,
        Emoji.WheelchairSymbol => EmojiStrings.WHEELCHAIR_SYMBOL,
        Emoji.Anchor => EmojiStrings.ANCHOR,
        Emoji.WarningSign => EmojiStrings.WARNING_SIGN,
        Emoji.HighVoltageSign => EmojiStrings.HIGH_VOLTAGE_SIGN,
        Emoji.MediumWhiteCircle => EmojiStrings.MEDIUM_WHITE_CIRCLE,
        Emoji.MediumBlackCircle => EmojiStrings.MEDIUM_BLACK_CIRCLE,
        Emoji.SoccerBall => EmojiStrings.SOCCER_BALL,
        Emoji.Baseball => EmojiStrings.BASEBALL,
        Emoji.SnowmanWithoutSnow => EmojiStrings.SNOWMAN_WITHOUT_SNOW,
        Emoji.SunBehindCloud => EmojiStrings.SUN_BEHIND_CLOUD,
        Emoji.Ophiuchus => EmojiStrings.OPHIUCHUS,
        Emoji.NoEntry => EmojiStrings.NO_ENTRY,
        Emoji.Church => EmojiStrings.CHURCH,
        Emoji.Fountain => EmojiStrings.FOUNTAIN,
        Emoji.FlagInHole => EmojiStrings.FLAG_IN_HOLE,
        Emoji.Sailboat => EmojiStrings.SAILBOAT,
        Emoji.Tent => EmojiStrings.TENT,
        Emoji.FuelPump => EmojiStrings.FUEL_PUMP,
        Emoji.ArrowPointingRightwardsThenCurvingUpwards => EmojiStrings.ARROW_POINTING_RIGHTWARDS_THEN_CURVING_UPWARDS,
        Emoji.ArrowPointingRightwardsThenCurvingDownwards => EmojiStrings.ARROW_POINTING_RIGHTWARDS_THEN_CURVING_DOWNWARDS,
        Emoji.LeftwardsBlackArrow => EmojiStrings.LEFTWARDS_BLACK_ARROW,
        Emoji.UpwardsBlackArrow => EmojiStrings.UPWARDS_BLACK_ARROW,
        Emoji.DownwardsBlackArrow => EmojiStrings.DOWNWARDS_BLACK_ARROW,
        Emoji.BlackLargeSquare => EmojiStrings.BLACK_LARGE_SQUARE,
        Emoji.WhiteLargeSquare => EmojiStrings.WHITE_LARGE_SQUARE,
        Emoji.WhiteMediumStar => EmojiStrings.WHITE_MEDIUM_STAR,
        Emoji.HeavyLargeCircle => EmojiStrings.HEAVY_LARGE_CIRCLE,
        Emoji.WavyDash => EmojiStrings.WAVY_DASH,
        Emoji.PartAlternationMark => EmojiStrings.PART_ALTERNATION_MARK,
        Emoji.CircledIdeographCongratulation => EmojiStrings.CIRCLED_IDEOGRAPH_CONGRATULATION,
        Emoji.CircledIdeographSecret => EmojiStrings.CIRCLED_IDEOGRAPH_SECRET,
        Emoji.MahjongTileRedDragon => EmojiStrings.MAHJONG_TILE_RED_DRAGON,
        Emoji.PlayingCardBlackJoker => EmojiStrings.PLAYING_CARD_BLACK_JOKER,
        Emoji.Cyclone => EmojiStrings.CYCLONE,
        Emoji.Foggy => EmojiStrings.FOGGY,
        Emoji.ClosedUmbrella => EmojiStrings.CLOSED_UMBRELLA,
        Emoji.NightWithStars => EmojiStrings.NIGHT_WITH_STARS,
        Emoji.SunriseOverMountains => EmojiStrings.SUNRISE_OVER_MOUNTAINS,
        Emoji.Sunrise => EmojiStrings.SUNRISE,
        Emoji.CityscapeAtDusk => EmojiStrings.CITYSCAPE_AT_DUSK,
        Emoji.SunsetOverBuildings => EmojiStrings.SUNSET_OVER_BUILDINGS,
        Emoji.Rainbow => EmojiStrings.RAINBOW,
        Emoji.BridgeAtNight => EmojiStrings.BRIDGE_AT_NIGHT,
        Emoji.WaterWave => EmojiStrings.WATER_WAVE,
        Emoji.Volcano => EmojiStrings.VOLCANO,
        Emoji.MilkyWay => EmojiStrings.MILKY_WAY,
        Emoji.EarthGlobeAsiaAustralia => EmojiStrings.EARTH_GLOBE_ASIA_AUSTRALIA,
        Emoji.NewMoonSymbol => EmojiStrings.NEW_MOON_SYMBOL,
        Emoji.FirstQuarterMoonSymbol => EmojiStrings.FIRST_QUARTER_MOON_SYMBOL,
        Emoji.WaxingGibbousMoonSymbol => EmojiStrings.WAXING_GIBBOUS_MOON_SYMBOL,
        Emoji.FullMoonSymbol => EmojiStrings.FULL_MOON_SYMBOL,
        Emoji.CrescentMoon => EmojiStrings.CRESCENT_MOON,
        Emoji.FirstQuarterMoonWithFace => EmojiStrings.FIRST_QUARTER_MOON_WITH_FACE,
        Emoji.GlowingStar => EmojiStrings.GLOWING_STAR,
        Emoji.ShootingStar => EmojiStrings.SHOOTING_STAR,
        Emoji.Chestnut => EmojiStrings.CHESTNUT,
        Emoji.Seedling => EmojiStrings.SEEDLING,
        Emoji.PalmTree => EmojiStrings.PALM_TREE,
        Emoji.Cactus => EmojiStrings.CACTUS,
        Emoji.Tulip => EmojiStrings.TULIP,
        Emoji.CherryBlossom => EmojiStrings.CHERRY_BLOSSOM,
        Emoji.Rose => EmojiStrings.ROSE,
        Emoji.Hibiscus => EmojiStrings.HIBISCUS,
        Emoji.Sunflower => EmojiStrings.SUNFLOWER,
        Emoji.Blossom => EmojiStrings.BLOSSOM,
        Emoji.EarOfMaize => EmojiStrings.EAR_OF_MAIZE,
        Emoji.EarOfRice => EmojiStrings.EAR_OF_RICE,
        Emoji.Herb => EmojiStrings.HERB,
        Emoji.FourLeafClover => EmojiStrings.FOUR_LEAF_CLOVER,
        Emoji.MapleLeaf => EmojiStrings.MAPLE_LEAF,
        Emoji.FallenLeaf => EmojiStrings.FALLEN_LEAF,
        Emoji.LeafFlutteringInWind => EmojiStrings.LEAF_FLUTTERING_IN_WIND,
        Emoji.Mushroom => EmojiStrings.MUSHROOM,
        Emoji.Tomato => EmojiStrings.TOMATO,
        Emoji.Aubergine => EmojiStrings.AUBERGINE,
        Emoji.Grapes => EmojiStrings.GRAPES,
        Emoji.Melon => EmojiStrings.MELON,
        Emoji.Watermelon => EmojiStrings.WATERMELON,
        Emoji.Tangerine => EmojiStrings.TANGERINE,
        Emoji.Banana => EmojiStrings.BANANA,
        Emoji.Pineapple => EmojiStrings.PINEAPPLE,
        Emoji.RedApple => EmojiStrings.RED_APPLE,
        Emoji.GreenApple => EmojiStrings.GREEN_APPLE,
        Emoji.Peach => EmojiStrings.PEACH,
        Emoji.Cherries => EmojiStrings.CHERRIES,
        Emoji.Strawberry => EmojiStrings.STRAWBERRY,
        Emoji.Hamburger => EmojiStrings.HAMBURGER,
        Emoji.SliceOfPizza => EmojiStrings.SLICE_OF_PIZZA,
        Emoji.MeatOnBone => EmojiStrings.MEAT_ON_BONE,
        Emoji.PoultryLeg => EmojiStrings.POULTRY_LEG,
        Emoji.RiceCracker => EmojiStrings.RICE_CRACKER,
        Emoji.RiceBall => EmojiStrings.RICE_BALL,
        Emoji.CookedRice => EmojiStrings.COOKED_RICE,
        Emoji.CurryAndRice => EmojiStrings.CURRY_AND_RICE,
        Emoji.SteamingBowl => EmojiStrings.STEAMING_BOWL,
        Emoji.Spaghetti => EmojiStrings.SPAGHETTI,
        Emoji.Bread => EmojiStrings.BREAD,
        Emoji.FrenchFries => EmojiStrings.FRENCH_FRIES,
        Emoji.RoastedSweetPotato => EmojiStrings.ROASTED_SWEET_POTATO,
        Emoji.Dango => EmojiStrings.DANGO,
        Emoji.Oden => EmojiStrings.ODEN,
        Emoji.Sushi => EmojiStrings.SUSHI,
        Emoji.FriedShrimp => EmojiStrings.FRIED_SHRIMP,
        Emoji.FishCakeWithSwirlDesign => EmojiStrings.FISH_CAKE_WITH_SWIRL_DESIGN,
        Emoji.SoftIceCream => EmojiStrings.SOFT_ICE_CREAM,
        Emoji.ShavedIce => EmojiStrings.SHAVED_ICE,
        Emoji.IceCream => EmojiStrings.ICE_CREAM,
        Emoji.Doughnut => EmojiStrings.DOUGHNUT,
        Emoji.Cookie => EmojiStrings.COOKIE,
        Emoji.ChocolateBar => EmojiStrings.CHOCOLATE_BAR,
        Emoji.Candy => EmojiStrings.CANDY,
        Emoji.Lollipop => EmojiStrings.LOLLIPOP,
        Emoji.Custard => EmojiStrings.CUSTARD,
        Emoji.HoneyPot => EmojiStrings.HONEY_POT,
        Emoji.Shortcake => EmojiStrings.SHORTCAKE,
        Emoji.BentoBox => EmojiStrings.BENTO_BOX,
        Emoji.PotOfFood => EmojiStrings.POT_OF_FOOD,
        Emoji.Cooking => EmojiStrings.COOKING,
        Emoji.ForkAndKnife => EmojiStrings.FORK_AND_KNIFE,
        Emoji.TeacupWithoutHandle => EmojiStrings.TEACUP_WITHOUT_HANDLE,
        Emoji.SakeBottleAndCup => EmojiStrings.SAKE_BOTTLE_AND_CUP,
        Emoji.WineGlass => EmojiStrings.WINE_GLASS,
        Emoji.CocktailGlass => EmojiStrings.COCKTAIL_GLASS,
        Emoji.TropicalDrink => EmojiStrings.TROPICAL_DRINK,
        Emoji.BeerMug => EmojiStrings.BEER_MUG,
        Emoji.ClinkingBeerMugs => EmojiStrings.CLINKING_BEER_MUGS,
        Emoji.Ribbon => EmojiStrings.RIBBON,
        Emoji.WrappedPresent => EmojiStrings.WRAPPED_PRESENT,
        Emoji.BirthdayCake => EmojiStrings.BIRTHDAY_CAKE,
        Emoji.JackOLantern => EmojiStrings.JACK_O_LANTERN,
        Emoji.ChristmasTree => EmojiStrings.CHRISTMAS_TREE,
        Emoji.FatherChristmas => EmojiStrings.FATHER_CHRISTMAS,
        Emoji.Fireworks => EmojiStrings.FIREWORKS,
        Emoji.FireworkSparkler => EmojiStrings.FIREWORK_SPARKLER,
        Emoji.Balloon => EmojiStrings.BALLOON,
        Emoji.PartyPopper => EmojiStrings.PARTY_POPPER,
        Emoji.ConfettiBall => EmojiStrings.CONFETTI_BALL,
        Emoji.TanabataTree => EmojiStrings.TANABATA_TREE,
        Emoji.CrossedFlags => EmojiStrings.CROSSED_FLAGS,
        Emoji.PineDecoration => EmojiStrings.PINE_DECORATION,
        Emoji.JapaneseDolls => EmojiStrings.JAPANESE_DOLLS,
        Emoji.CarpStreamer => EmojiStrings.CARP_STREAMER,
        Emoji.WindChime => EmojiStrings.WIND_CHIME,
        Emoji.MoonViewingCeremony => EmojiStrings.MOON_VIEWING_CEREMONY,
        Emoji.SchoolSatchel => EmojiStrings.SCHOOL_SATCHEL,
        Emoji.GraduationCap => EmojiStrings.GRADUATION_CAP,
        Emoji.CarouselHorse => EmojiStrings.CAROUSEL_HORSE,
        Emoji.FerrisWheel => EmojiStrings.FERRIS_WHEEL,
        Emoji.RollerCoaster => EmojiStrings.ROLLER_COASTER,
        Emoji.FishingPoleAndFish => EmojiStrings.FISHING_POLE_AND_FISH,
        Emoji.Microphone => EmojiStrings.MICROPHONE,
        Emoji.MovieCamera => EmojiStrings.MOVIE_CAMERA,
        Emoji.Cinema => EmojiStrings.CINEMA,
        Emoji.Headphone => EmojiStrings.HEADPHONE,
        Emoji.ArtistPalette => EmojiStrings.ARTIST_PALETTE,
        Emoji.TopHat => EmojiStrings.TOP_HAT,
        Emoji.CircusTent => EmojiStrings.CIRCUS_TENT,
        Emoji.Ticket => EmojiStrings.TICKET,
        Emoji.ClapperBoard => EmojiStrings.CLAPPER_BOARD,
        Emoji.PerformingArts => EmojiStrings.PERFORMING_ARTS,
        Emoji.VideoGame => EmojiStrings.VIDEO_GAME,
        Emoji.DirectHit => EmojiStrings.DIRECT_HIT,
        Emoji.SlotMachine => EmojiStrings.SLOT_MACHINE,
        Emoji.Billiards => EmojiStrings.BILLIARDS,
        Emoji.GameDie => EmojiStrings.GAME_DIE,
        Emoji.Bowling => EmojiStrings.BOWLING,
        Emoji.FlowerPlayingCards => EmojiStrings.FLOWER_PLAYING_CARDS,
        Emoji.MusicalNote => EmojiStrings.MUSICAL_NOTE,
        Emoji.MultipleMusicalNotes => EmojiStrings.MULTIPLE_MUSICAL_NOTES,
        Emoji.Saxophone => EmojiStrings.SAXOPHONE,
        Emoji.Guitar => EmojiStrings.GUITAR,
        Emoji.MusicalKeyboard => EmojiStrings.MUSICAL_KEYBOARD,
        Emoji.Trumpet => EmojiStrings.TRUMPET,
        Emoji.Violin => EmojiStrings.VIOLIN,
        Emoji.MusicalScore => EmojiStrings.MUSICAL_SCORE,
        Emoji.RunningShirtWithSash => EmojiStrings.RUNNING_SHIRT_WITH_SASH,
        Emoji.TennisRacquetAndBall => EmojiStrings.TENNIS_RACQUET_AND_BALL,
        Emoji.SkiAndSkiBoot => EmojiStrings.SKI_AND_SKI_BOOT,
        Emoji.BasketballAndHoop => EmojiStrings.BASKETBALL_AND_HOOP,
        Emoji.ChequeredFlag => EmojiStrings.CHEQUERED_FLAG,
        Emoji.Snowboarder => EmojiStrings.SNOWBOARDER,
        Emoji.Runner => EmojiStrings.RUNNER,
        Emoji.Surfer => EmojiStrings.SURFER,
        Emoji.Trophy => EmojiStrings.TROPHY,
        Emoji.AmericanFootball => EmojiStrings.AMERICAN_FOOTBALL,
        Emoji.Swimmer => EmojiStrings.SWIMMER,
        Emoji.HouseBuilding => EmojiStrings.HOUSE_BUILDING,
        Emoji.HouseWithGarden => EmojiStrings.HOUSE_WITH_GARDEN,
        Emoji.OfficeBuilding => EmojiStrings.OFFICE_BUILDING,
        Emoji.JapanesePostOffice => EmojiStrings.JAPANESE_POST_OFFICE,
        Emoji.Hospital => EmojiStrings.HOSPITAL,
        Emoji.Bank => EmojiStrings.BANK,
        Emoji.AutomatedTellerMachine => EmojiStrings.AUTOMATED_TELLER_MACHINE,
        Emoji.Hotel => EmojiStrings.HOTEL,
        Emoji.LoveHotel => EmojiStrings.LOVE_HOTEL,
        Emoji.ConvenienceStore => EmojiStrings.CONVENIENCE_STORE,
        Emoji.School => EmojiStrings.SCHOOL,
        Emoji.DepartmentStore => EmojiStrings.DEPARTMENT_STORE,
        Emoji.Factory => EmojiStrings.FACTORY,
        Emoji.IzakayaLantern => EmojiStrings.IZAKAYA_LANTERN,
        Emoji.JapaneseCastle => EmojiStrings.JAPANESE_CASTLE,
        Emoji.EuropeanCastle => EmojiStrings.EUROPEAN_CASTLE,
        Emoji.Snail => EmojiStrings.SNAIL,
        Emoji.Snake => EmojiStrings.SNAKE,
        Emoji.Horse => EmojiStrings.HORSE,
        Emoji.Sheep => EmojiStrings.SHEEP,
        Emoji.Monkey => EmojiStrings.MONKEY,
        Emoji.Chicken => EmojiStrings.CHICKEN,
        Emoji.Boar => EmojiStrings.BOAR,
        Emoji.Elephant => EmojiStrings.ELEPHANT,
        Emoji.Octopus => EmojiStrings.OCTOPUS,
        Emoji.SpiralShell => EmojiStrings.SPIRAL_SHELL,
        Emoji.Bug => EmojiStrings.BUG,
        Emoji.Ant => EmojiStrings.ANT,
        Emoji.Honeybee => EmojiStrings.HONEYBEE,
        Emoji.LadyBeetle => EmojiStrings.LADY_BEETLE,
        Emoji.Fish => EmojiStrings.FISH,
        Emoji.TropicalFish => EmojiStrings.TROPICAL_FISH,
        Emoji.Blowfish => EmojiStrings.BLOWFISH,
        Emoji.Turtle => EmojiStrings.TURTLE,
        Emoji.HatchingChick => EmojiStrings.HATCHING_CHICK,
        Emoji.BabyChick => EmojiStrings.BABY_CHICK,
        Emoji.FrontFacingBabyChick => EmojiStrings.FRONT_FACING_BABY_CHICK,
        Emoji.Bird => EmojiStrings.BIRD,
        Emoji.Penguin => EmojiStrings.PENGUIN,
        Emoji.Koala => EmojiStrings.KOALA,
        Emoji.Poodle => EmojiStrings.POODLE,
        Emoji.BactrianCamel => EmojiStrings.BACTRIAN_CAMEL,
        Emoji.Dolphin => EmojiStrings.DOLPHIN,
        Emoji.MouseFace => EmojiStrings.MOUSE_FACE,
        Emoji.CowFace => EmojiStrings.COW_FACE,
        Emoji.TigerFace => EmojiStrings.TIGER_FACE,
        Emoji.RabbitFace => EmojiStrings.RABBIT_FACE,
        Emoji.CatFace => EmojiStrings.CAT_FACE,
        Emoji.DragonFace => EmojiStrings.DRAGON_FACE,
        Emoji.SpoutingWhale => EmojiStrings.SPOUTING_WHALE,
        Emoji.HorseFace => EmojiStrings.HORSE_FACE,
        Emoji.MonkeyFace => EmojiStrings.MONKEY_FACE,
        Emoji.DogFace => EmojiStrings.DOG_FACE,
        Emoji.PigFace => EmojiStrings.PIG_FACE,
        Emoji.FrogFace => EmojiStrings.FROG_FACE,
        Emoji.HamsterFace => EmojiStrings.HAMSTER_FACE,
        Emoji.WolfFace => EmojiStrings.WOLF_FACE,
        Emoji.BearFace => EmojiStrings.BEAR_FACE,
        Emoji.PandaFace => EmojiStrings.PANDA_FACE,
        Emoji.PigNose => EmojiStrings.PIG_NOSE,
        Emoji.PawPrints => EmojiStrings.PAW_PRINTS,
        Emoji.Eyes => EmojiStrings.EYES,
        Emoji.Ear => EmojiStrings.EAR,
        Emoji.Nose => EmojiStrings.NOSE,
        Emoji.Mouth => EmojiStrings.MOUTH,
        Emoji.Tongue => EmojiStrings.TONGUE,
        Emoji.WhiteUpPointingBackhandIndex => EmojiStrings.WHITE_UP_POINTING_BACKHAND_INDEX,
        Emoji.WhiteDownPointingBackhandIndex => EmojiStrings.WHITE_DOWN_POINTING_BACKHAND_INDEX,
        Emoji.WhiteLeftPointingBackhandIndex => EmojiStrings.WHITE_LEFT_POINTING_BACKHAND_INDEX,
        Emoji.WhiteRightPointingBackhandIndex => EmojiStrings.WHITE_RIGHT_POINTING_BACKHAND_INDEX,
        Emoji.FistedHandSign => EmojiStrings.FISTED_HAND_SIGN,
        Emoji.WavingHandSign => EmojiStrings.WAVING_HAND_SIGN,
        Emoji.OkHandSign => EmojiStrings.OK_HAND_SIGN,
        Emoji.ThumbsUpSign => EmojiStrings.THUMBS_UP_SIGN,
        Emoji.ThumbsDownSign => EmojiStrings.THUMBS_DOWN_SIGN,
        Emoji.ClappingHandsSign => EmojiStrings.CLAPPING_HANDS_SIGN,
        Emoji.OpenHandsSign => EmojiStrings.OPEN_HANDS_SIGN,
        Emoji.Crown => EmojiStrings.CROWN,
        Emoji.WomansHat => EmojiStrings.WOMANS_HAT,
        Emoji.Eyeglasses => EmojiStrings.EYEGLASSES,
        Emoji.Necktie => EmojiStrings.NECKTIE,
        Emoji.TShirt => EmojiStrings.T_SHIRT,
        Emoji.Jeans => EmojiStrings.JEANS,
        Emoji.Dress => EmojiStrings.DRESS,
        Emoji.Kimono => EmojiStrings.KIMONO,
        Emoji.Bikini => EmojiStrings.BIKINI,
        Emoji.WomansClothes => EmojiStrings.WOMANS_CLOTHES,
        Emoji.Purse => EmojiStrings.PURSE,
        Emoji.Handbag => EmojiStrings.HANDBAG,
        Emoji.Pouch => EmojiStrings.POUCH,
        Emoji.MansShoe => EmojiStrings.MANS_SHOE,
        Emoji.AthleticShoe => EmojiStrings.ATHLETIC_SHOE,
        Emoji.HighHeeledShoe => EmojiStrings.HIGH_HEELED_SHOE,
        Emoji.WomansSandal => EmojiStrings.WOMANS_SANDAL,
        Emoji.WomansBoots => EmojiStrings.WOMANS_BOOTS,
        Emoji.Footprints => EmojiStrings.FOOTPRINTS,
        Emoji.BustInSilhouette => EmojiStrings.BUST_IN_SILHOUETTE,
        Emoji.Boy => EmojiStrings.BOY,
        Emoji.Girl => EmojiStrings.GIRL,
        Emoji.Man => EmojiStrings.MAN,
        Emoji.Woman => EmojiStrings.WOMAN,
        Emoji.Family => EmojiStrings.FAMILY,
        Emoji.ManAndWomanHoldingHands => EmojiStrings.MAN_AND_WOMAN_HOLDING_HANDS,
        Emoji.PoliceOfficer => EmojiStrings.POLICE_OFFICER,
        Emoji.WomanWithBunnyEars => EmojiStrings.WOMAN_WITH_BUNNY_EARS,
        Emoji.BrideWithVeil => EmojiStrings.BRIDE_WITH_VEIL,
        Emoji.PersonWithBlondHair => EmojiStrings.PERSON_WITH_BLOND_HAIR,
        Emoji.ManWithGuaPiMao => EmojiStrings.MAN_WITH_GUA_PI_MAO,
        Emoji.ManWithTurban => EmojiStrings.MAN_WITH_TURBAN,
        Emoji.OlderMan => EmojiStrings.OLDER_MAN,
        Emoji.OlderWoman => EmojiStrings.OLDER_WOMAN,
        Emoji.Baby => EmojiStrings.BABY,
        Emoji.ConstructionWorker => EmojiStrings.CONSTRUCTION_WORKER,
        Emoji.Princess => EmojiStrings.PRINCESS,
        Emoji.JapaneseOgre => EmojiStrings.JAPANESE_OGRE,
        Emoji.JapaneseGoblin => EmojiStrings.JAPANESE_GOBLIN,
        Emoji.Ghost => EmojiStrings.GHOST,
        Emoji.BabyAngel => EmojiStrings.BABY_ANGEL,
        Emoji.ExtraterrestrialAlien => EmojiStrings.EXTRATERRESTRIAL_ALIEN,
        Emoji.AlienMonster => EmojiStrings.ALIEN_MONSTER,
        Emoji.Imp => EmojiStrings.IMP,
        Emoji.Skull => EmojiStrings.SKULL,
        Emoji.InformationDeskPerson => EmojiStrings.INFORMATION_DESK_PERSON,
        Emoji.Guardsman => EmojiStrings.GUARDSMAN,
        Emoji.Dancer => EmojiStrings.DANCER,
        Emoji.Lipstick => EmojiStrings.LIPSTICK,
        Emoji.NailPolish => EmojiStrings.NAIL_POLISH,
        Emoji.FaceMassage => EmojiStrings.FACE_MASSAGE,
        Emoji.Haircut => EmojiStrings.HAIRCUT,
        Emoji.BarberPole => EmojiStrings.BARBER_POLE,
        Emoji.Syringe => EmojiStrings.SYRINGE,
        Emoji.Pill => EmojiStrings.PILL,
        Emoji.KissMark => EmojiStrings.KISS_MARK,
        Emoji.LoveLetter => EmojiStrings.LOVE_LETTER,
        Emoji.Ring => EmojiStrings.RING,
        Emoji.GemStone => EmojiStrings.GEM_STONE,
        Emoji.Kiss => EmojiStrings.KISS,
        Emoji.Bouquet => EmojiStrings.BOUQUET,
        Emoji.CoupleWithHeart => EmojiStrings.COUPLE_WITH_HEART,
        Emoji.Wedding => EmojiStrings.WEDDING,
        Emoji.BeatingHeart => EmojiStrings.BEATING_HEART,
        Emoji.BrokenHeart => EmojiStrings.BROKEN_HEART,
        Emoji.TwoHearts => EmojiStrings.TWO_HEARTS,
        Emoji.SparklingHeart => EmojiStrings.SPARKLING_HEART,
        Emoji.GrowingHeart => EmojiStrings.GROWING_HEART,
        Emoji.HeartWithArrow => EmojiStrings.HEART_WITH_ARROW,
        Emoji.BlueHeart => EmojiStrings.BLUE_HEART,
        Emoji.GreenHeart => EmojiStrings.GREEN_HEART,
        Emoji.YellowHeart => EmojiStrings.YELLOW_HEART,
        Emoji.PurpleHeart => EmojiStrings.PURPLE_HEART,
        Emoji.HeartWithRibbon => EmojiStrings.HEART_WITH_RIBBON,
        Emoji.RevolvingHearts => EmojiStrings.REVOLVING_HEARTS,
        Emoji.HeartDecoration => EmojiStrings.HEART_DECORATION,
        Emoji.DiamondShapeWithADotInside => EmojiStrings.DIAMOND_SHAPE_WITH_A_DOT_INSIDE,
        Emoji.ElectricLightBulb => EmojiStrings.ELECTRIC_LIGHT_BULB,
        Emoji.AngerSymbol => EmojiStrings.ANGER_SYMBOL,
        Emoji.Bomb => EmojiStrings.BOMB,
        Emoji.SleepingSymbol => EmojiStrings.SLEEPING_SYMBOL,
        Emoji.CollisionSymbol => EmojiStrings.COLLISION_SYMBOL,
        Emoji.SplashingSweatSymbol => EmojiStrings.SPLASHING_SWEAT_SYMBOL,
        Emoji.Droplet => EmojiStrings.DROPLET,
        Emoji.DashSymbol => EmojiStrings.DASH_SYMBOL,
        Emoji.PileOfPoo => EmojiStrings.PILE_OF_POO,
        Emoji.FlexedBiceps => EmojiStrings.FLEXED_BICEPS,
        Emoji.DizzySymbol => EmojiStrings.DIZZY_SYMBOL,
        Emoji.SpeechBalloon => EmojiStrings.SPEECH_BALLOON,
        Emoji.WhiteFlower => EmojiStrings.WHITE_FLOWER,
        Emoji.HundredPointsSymbol => EmojiStrings.HUNDRED_POINTS_SYMBOL,
        Emoji.MoneyBag => EmojiStrings.MONEY_BAG,
        Emoji.CurrencyExchange => EmojiStrings.CURRENCY_EXCHANGE,
        Emoji.HeavyDollarSign => EmojiStrings.HEAVY_DOLLAR_SIGN,
        Emoji.CreditCard => EmojiStrings.CREDIT_CARD,
        Emoji.BanknoteWithYenSign => EmojiStrings.BANKNOTE_WITH_YEN_SIGN,
        Emoji.BanknoteWithDollarSign => EmojiStrings.BANKNOTE_WITH_DOLLAR_SIGN,
        Emoji.MoneyWithWings => EmojiStrings.MONEY_WITH_WINGS,
        Emoji.ChartWithUpwardsTrendAndYenSign => EmojiStrings.CHART_WITH_UPWARDS_TREND_AND_YEN_SIGN,
        Emoji.Seat => EmojiStrings.SEAT,
        Emoji.PersonalComputer => EmojiStrings.PERSONAL_COMPUTER,
        Emoji.Briefcase => EmojiStrings.BRIEFCASE,
        Emoji.Minidisc => EmojiStrings.MINIDISC,
        Emoji.FloppyDisk => EmojiStrings.FLOPPY_DISK,
        Emoji.OpticalDisc => EmojiStrings.OPTICAL_DISC,
        Emoji.Dvd => EmojiStrings.DVD,
        Emoji.FileFolder => EmojiStrings.FILE_FOLDER,
        Emoji.OpenFileFolder => EmojiStrings.OPEN_FILE_FOLDER,
        Emoji.PageWithCurl => EmojiStrings.PAGE_WITH_CURL,
        Emoji.PageFacingUp => EmojiStrings.PAGE_FACING_UP,
        Emoji.Calendar => EmojiStrings.CALENDAR,
        Emoji.TearOffCalendar => EmojiStrings.TEAR_OFF_CALENDAR,
        Emoji.CardIndex => EmojiStrings.CARD_INDEX,
        Emoji.ChartWithUpwardsTrend => EmojiStrings.CHART_WITH_UPWARDS_TREND,
        Emoji.ChartWithDownwardsTrend => EmojiStrings.CHART_WITH_DOWNWARDS_TREND,
        Emoji.BarChart => EmojiStrings.BAR_CHART,
        Emoji.Clipboard => EmojiStrings.CLIPBOARD,
        Emoji.Pushpin => EmojiStrings.PUSHPIN,
        Emoji.RoundPushpin => EmojiStrings.ROUND_PUSHPIN,
        Emoji.Paperclip => EmojiStrings.PAPERCLIP,
        Emoji.StraightRuler => EmojiStrings.STRAIGHT_RULER,
        Emoji.TriangularRuler => EmojiStrings.TRIANGULAR_RULER,
        Emoji.BookmarkTabs => EmojiStrings.BOOKMARK_TABS,
        Emoji.Ledger => EmojiStrings.LEDGER,
        Emoji.Notebook => EmojiStrings.NOTEBOOK,
        Emoji.NotebookWithDecorativeCover => EmojiStrings.NOTEBOOK_WITH_DECORATIVE_COVER,
        Emoji.ClosedBook => EmojiStrings.CLOSED_BOOK,
        Emoji.OpenBook => EmojiStrings.OPEN_BOOK,
        Emoji.GreenBook => EmojiStrings.GREEN_BOOK,
        Emoji.BlueBook => EmojiStrings.BLUE_BOOK,
        Emoji.OrangeBook => EmojiStrings.ORANGE_BOOK,
        Emoji.Books => EmojiStrings.BOOKS,
        Emoji.NameBadge => EmojiStrings.NAME_BADGE,
        Emoji.Scroll => EmojiStrings.SCROLL,
        Emoji.Memo => EmojiStrings.MEMO,
        Emoji.TelephoneReceiver => EmojiStrings.TELEPHONE_RECEIVER,
        Emoji.Pager => EmojiStrings.PAGER,
        Emoji.FaxMachine => EmojiStrings.FAX_MACHINE,
        Emoji.SatelliteAntenna => EmojiStrings.SATELLITE_ANTENNA,
        Emoji.PublicAddressLoudspeaker => EmojiStrings.PUBLIC_ADDRESS_LOUDSPEAKER,
        Emoji.CheeringMegaphone => EmojiStrings.CHEERING_MEGAPHONE,
        Emoji.OutboxTray => EmojiStrings.OUTBOX_TRAY,
        Emoji.InboxTray => EmojiStrings.INBOX_TRAY,
        Emoji.Package => EmojiStrings.PACKAGE,
        Emoji.EMailSymbol => EmojiStrings.E_MAIL_SYMBOL,
        Emoji.IncomingEnvelope => EmojiStrings.INCOMING_ENVELOPE,
        Emoji.EnvelopeWithDownwardsArrowAbove => EmojiStrings.ENVELOPE_WITH_DOWNWARDS_ARROW_ABOVE,
        Emoji.ClosedMailboxWithLoweredFlag => EmojiStrings.CLOSED_MAILBOX_WITH_LOWERED_FLAG,
        Emoji.ClosedMailboxWithRaisedFlag => EmojiStrings.CLOSED_MAILBOX_WITH_RAISED_FLAG,
        Emoji.Postbox => EmojiStrings.POSTBOX,
        Emoji.Newspaper => EmojiStrings.NEWSPAPER,
        Emoji.MobilePhone => EmojiStrings.MOBILE_PHONE,
        Emoji.MobilePhoneWithRightwardsArrowAtLeft => EmojiStrings.MOBILE_PHONE_WITH_RIGHTWARDS_ARROW_AT_LEFT,
        Emoji.VibrationMode => EmojiStrings.VIBRATION_MODE,
        Emoji.MobilePhoneOff => EmojiStrings.MOBILE_PHONE_OFF,
        Emoji.AntennaWithBars => EmojiStrings.ANTENNA_WITH_BARS,
        Emoji.Camera => EmojiStrings.CAMERA,
        Emoji.VideoCamera => EmojiStrings.VIDEO_CAMERA,
        Emoji.Television => EmojiStrings.TELEVISION,
        Emoji.Radio => EmojiStrings.RADIO,
        Emoji.Videocassette => EmojiStrings.VIDEOCASSETTE,
        Emoji.ClockwiseDownwardsAndUpwardsOpenCircleArrows => EmojiStrings.CLOCKWISE_DOWNWARDS_AND_UPWARDS_OPEN_CIRCLE_ARROWS,
        Emoji.SpeakerWithThreeSoundWaves => EmojiStrings.SPEAKER_WITH_THREE_SOUND_WAVES,
        Emoji.Battery => EmojiStrings.BATTERY,
        Emoji.ElectricPlug => EmojiStrings.ELECTRIC_PLUG,
        Emoji.LeftPointingMagnifyingGlass => EmojiStrings.LEFT_POINTING_MAGNIFYING_GLASS,
        Emoji.RightPointingMagnifyingGlass => EmojiStrings.RIGHT_POINTING_MAGNIFYING_GLASS,
        Emoji.LockWithInkPen => EmojiStrings.LOCK_WITH_INK_PEN,
        Emoji.ClosedLockWithKey => EmojiStrings.CLOSED_LOCK_WITH_KEY,
        Emoji.Key => EmojiStrings.KEY,
        Emoji.Lock => EmojiStrings.LOCK,
        Emoji.OpenLock => EmojiStrings.OPEN_LOCK,
        Emoji.Bell => EmojiStrings.BELL,
        Emoji.Bookmark => EmojiStrings.BOOKMARK,
        Emoji.LinkSymbol => EmojiStrings.LINK_SYMBOL,
        Emoji.RadioButton => EmojiStrings.RADIO_BUTTON,
        Emoji.BackWithLeftwardsArrowAbove => EmojiStrings.BACK_WITH_LEFTWARDS_ARROW_ABOVE,
        Emoji.EndWithLeftwardsArrowAbove => EmojiStrings.END_WITH_LEFTWARDS_ARROW_ABOVE,
        Emoji.OnWithExclamationMarkWithLeftRightArrowAbove => EmojiStrings.ON_WITH_EXCLAMATION_MARK_WITH_LEFT_RIGHT_ARROW_ABOVE,
        Emoji.SoonWithRightwardsArrowAbove => EmojiStrings.SOON_WITH_RIGHTWARDS_ARROW_ABOVE,
        Emoji.TopWithUpwardsArrowAbove => EmojiStrings.TOP_WITH_UPWARDS_ARROW_ABOVE,
        Emoji.NoOneUnderEighteenSymbol => EmojiStrings.NO_ONE_UNDER_EIGHTEEN_SYMBOL,
        Emoji.KeycapTen => EmojiStrings.KEYCAP_TEN,
        Emoji.InputSymbolForLatinCapitalLetters => EmojiStrings.INPUT_SYMBOL_FOR_LATIN_CAPITAL_LETTERS,
        Emoji.InputSymbolForLatinSmallLetters => EmojiStrings.INPUT_SYMBOL_FOR_LATIN_SMALL_LETTERS,
        Emoji.InputSymbolForNumbers => EmojiStrings.INPUT_SYMBOL_FOR_NUMBERS,
        Emoji.InputSymbolForSymbols => EmojiStrings.INPUT_SYMBOL_FOR_SYMBOLS,
        Emoji.InputSymbolForLatinLetters => EmojiStrings.INPUT_SYMBOL_FOR_LATIN_LETTERS,
        Emoji.Fire => EmojiStrings.FIRE,
        Emoji.ElectricTorch => EmojiStrings.ELECTRIC_TORCH,
        Emoji.Wrench => EmojiStrings.WRENCH,
        Emoji.Hammer => EmojiStrings.HAMMER,
        Emoji.NutAndBolt => EmojiStrings.NUT_AND_BOLT,
        Emoji.Hocho => EmojiStrings.HOCHO,
        Emoji.Pistol => EmojiStrings.PISTOL,
        Emoji.CrystalBall => EmojiStrings.CRYSTAL_BALL,
        Emoji.SixPointedStarWithMiddleDot => EmojiStrings.SIX_POINTED_STAR_WITH_MIDDLE_DOT,
        Emoji.JapaneseSymbolForBeginner => EmojiStrings.JAPANESE_SYMBOL_FOR_BEGINNER,
        Emoji.TridentEmblem => EmojiStrings.TRIDENT_EMBLEM,
        Emoji.BlackSquareButton => EmojiStrings.BLACK_SQUARE_BUTTON,
        Emoji.WhiteSquareButton => EmojiStrings.WHITE_SQUARE_BUTTON,
        Emoji.LargeRedCircle => EmojiStrings.LARGE_RED_CIRCLE,
        Emoji.LargeBlueCircle => EmojiStrings.LARGE_BLUE_CIRCLE,
        Emoji.LargeOrangeDiamond => EmojiStrings.LARGE_ORANGE_DIAMOND,
        Emoji.LargeBlueDiamond => EmojiStrings.LARGE_BLUE_DIAMOND,
        Emoji.SmallOrangeDiamond => EmojiStrings.SMALL_ORANGE_DIAMOND,
        Emoji.SmallBlueDiamond => EmojiStrings.SMALL_BLUE_DIAMOND,
        Emoji.UpPointingRedTriangle => EmojiStrings.UP_POINTING_RED_TRIANGLE,
        Emoji.DownPointingRedTriangle => EmojiStrings.DOWN_POINTING_RED_TRIANGLE,
        Emoji.UpPointingSmallRedTriangle => EmojiStrings.UP_POINTING_SMALL_RED_TRIANGLE,
        Emoji.DownPointingSmallRedTriangle => EmojiStrings.DOWN_POINTING_SMALL_RED_TRIANGLE,
        Emoji.ClockFaceOneOclock => EmojiStrings.CLOCK_FACE_ONE_OCLOCK,
        Emoji.ClockFaceTwoOclock => EmojiStrings.CLOCK_FACE_TWO_OCLOCK,
        Emoji.ClockFaceThreeOclock => EmojiStrings.CLOCK_FACE_THREE_OCLOCK,
        Emoji.ClockFaceFourOclock => EmojiStrings.CLOCK_FACE_FOUR_OCLOCK,
        Emoji.ClockFaceFiveOclock => EmojiStrings.CLOCK_FACE_FIVE_OCLOCK,
        Emoji.ClockFaceSixOclock => EmojiStrings.CLOCK_FACE_SIX_OCLOCK,
        Emoji.ClockFaceSevenOclock => EmojiStrings.CLOCK_FACE_SEVEN_OCLOCK,
        Emoji.ClockFaceEightOclock => EmojiStrings.CLOCK_FACE_EIGHT_OCLOCK,
        Emoji.ClockFaceNineOclock => EmojiStrings.CLOCK_FACE_NINE_OCLOCK,
        Emoji.ClockFaceTenOclock => EmojiStrings.CLOCK_FACE_TEN_OCLOCK,
        Emoji.ClockFaceElevenOclock => EmojiStrings.CLOCK_FACE_ELEVEN_OCLOCK,
        Emoji.ClockFaceTwelveOclock => EmojiStrings.CLOCK_FACE_TWELVE_OCLOCK,
        Emoji.MountFuji => EmojiStrings.MOUNT_FUJI,
        Emoji.TokyoTower => EmojiStrings.TOKYO_TOWER,
        Emoji.StatueOfLiberty => EmojiStrings.STATUE_OF_LIBERTY,
        Emoji.SilhouetteOfJapan => EmojiStrings.SILHOUETTE_OF_JAPAN,
        Emoji.Moyai => EmojiStrings.MOYAI,
        Emoji.GrinningFace => EmojiStrings.GRINNING_FACE,
        Emoji.SmilingFaceWithHalo => EmojiStrings.SMILING_FACE_WITH_HALO,
        Emoji.SmilingFaceWithHorns => EmojiStrings.SMILING_FACE_WITH_HORNS,
        Emoji.SmilingFaceWithSunglasses => EmojiStrings.SMILING_FACE_WITH_SUNGLASSES,
        Emoji.NeutralFace => EmojiStrings.NEUTRAL_FACE,
        Emoji.ExpressionlessFace => EmojiStrings.EXPRESSIONLESS_FACE,
        Emoji.ConfusedFace => EmojiStrings.CONFUSED_FACE,
        Emoji.KissingFace => EmojiStrings.KISSING_FACE,
        Emoji.KissingFaceWithSmilingEyes => EmojiStrings.KISSING_FACE_WITH_SMILING_EYES,
        Emoji.FaceWithStuckOutTongue => EmojiStrings.FACE_WITH_STUCK_OUT_TONGUE,
        Emoji.WorriedFace => EmojiStrings.WORRIED_FACE,
        Emoji.FrowningFaceWithOpenMouth => EmojiStrings.FROWNING_FACE_WITH_OPEN_MOUTH,
        Emoji.AnguishedFace => EmojiStrings.ANGUISHED_FACE,
        Emoji.GrimacingFace => EmojiStrings.GRIMACING_FACE,
        Emoji.FaceWithOpenMouth => EmojiStrings.FACE_WITH_OPEN_MOUTH,
        Emoji.HushedFace => EmojiStrings.HUSHED_FACE,
        Emoji.SleepingFace => EmojiStrings.SLEEPING_FACE,
        Emoji.FaceWithoutMouth => EmojiStrings.FACE_WITHOUT_MOUTH,
        Emoji.Helicopter => EmojiStrings.HELICOPTER,
        Emoji.SteamLocomotive => EmojiStrings.STEAM_LOCOMOTIVE,
        Emoji.Train => EmojiStrings.TRAIN,
        Emoji.LightRail => EmojiStrings.LIGHT_RAIL,
        Emoji.Tram => EmojiStrings.TRAM,
        Emoji.OncomingBus => EmojiStrings.ONCOMING_BUS,
        Emoji.Trolleybus => EmojiStrings.TROLLEYBUS,
        Emoji.Minibus => EmojiStrings.MINIBUS,
        Emoji.OncomingPoliceCar => EmojiStrings.ONCOMING_POLICE_CAR,
        Emoji.OncomingTaxi => EmojiStrings.ONCOMING_TAXI,
        Emoji.OncomingAutomobile => EmojiStrings.ONCOMING_AUTOMOBILE,
        Emoji.ArticulatedLorry => EmojiStrings.ARTICULATED_LORRY,
        Emoji.Tractor => EmojiStrings.TRACTOR,
        Emoji.Monorail => EmojiStrings.MONORAIL,
        Emoji.MountainRailway => EmojiStrings.MOUNTAIN_RAILWAY,
        Emoji.SuspensionRailway => EmojiStrings.SUSPENSION_RAILWAY,
        Emoji.MountainCableway => EmojiStrings.MOUNTAIN_CABLEWAY,
        Emoji.AerialTramway => EmojiStrings.AERIAL_TRAMWAY,
        Emoji.Rowboat => EmojiStrings.ROWBOAT,
        Emoji.VerticalTrafficLight => EmojiStrings.VERTICAL_TRAFFIC_LIGHT,
        Emoji.PutLitterInItsPlaceSymbol => EmojiStrings.PUT_LITTER_IN_ITS_PLACE_SYMBOL,
        Emoji.DoNotLitterSymbol => EmojiStrings.DO_NOT_LITTER_SYMBOL,
        Emoji.PotableWaterSymbol => EmojiStrings.POTABLE_WATER_SYMBOL,
        Emoji.NonPotableWaterSymbol => EmojiStrings.NON_POTABLE_WATER_SYMBOL,
        Emoji.NoBicycles => EmojiStrings.NO_BICYCLES,
        Emoji.Bicyclist => EmojiStrings.BICYCLIST,
        Emoji.MountainBicyclist => EmojiStrings.MOUNTAIN_BICYCLIST,
        Emoji.NoPedestrians => EmojiStrings.NO_PEDESTRIANS,
        Emoji.ChildrenCrossing => EmojiStrings.CHILDREN_CROSSING,
        Emoji.Shower => EmojiStrings.SHOWER,
        Emoji.Bathtub => EmojiStrings.BATHTUB,
        Emoji.PassportControl => EmojiStrings.PASSPORT_CONTROL,
        Emoji.Customs => EmojiStrings.CUSTOMS,
        Emoji.BaggageClaim => EmojiStrings.BAGGAGE_CLAIM,
        Emoji.LeftLuggage => EmojiStrings.LEFT_LUGGAGE,
        Emoji.EarthGlobeEuropeAfrica => EmojiStrings.EARTH_GLOBE_EUROPE_AFRICA,
        Emoji.EarthGlobeAmericas => EmojiStrings.EARTH_GLOBE_AMERICAS,
        Emoji.GlobeWithMeridians => EmojiStrings.GLOBE_WITH_MERIDIANS,
        Emoji.WaxingCrescentMoonSymbol => EmojiStrings.WAXING_CRESCENT_MOON_SYMBOL,
        Emoji.WaningGibbousMoonSymbol => EmojiStrings.WANING_GIBBOUS_MOON_SYMBOL,
        Emoji.LastQuarterMoonSymbol => EmojiStrings.LAST_QUARTER_MOON_SYMBOL,
        Emoji.WaningCrescentMoonSymbol => EmojiStrings.WANING_CRESCENT_MOON_SYMBOL,
        Emoji.NewMoonWithFace => EmojiStrings.NEW_MOON_WITH_FACE,
        Emoji.LastQuarterMoonWithFace => EmojiStrings.LAST_QUARTER_MOON_WITH_FACE,
        Emoji.FullMoonWithFace => EmojiStrings.FULL_MOON_WITH_FACE,
        Emoji.SunWithFace => EmojiStrings.SUN_WITH_FACE,
        Emoji.EvergreenTree => EmojiStrings.EVERGREEN_TREE,
        Emoji.DeciduousTree => EmojiStrings.DECIDUOUS_TREE,
        Emoji.Lemon => EmojiStrings.LEMON,
        Emoji.Pear => EmojiStrings.PEAR,
        Emoji.BabyBottle => EmojiStrings.BABY_BOTTLE,
        Emoji.HorseRacing => EmojiStrings.HORSE_RACING,
        Emoji.RugbyFootball => EmojiStrings.RUGBY_FOOTBALL,
        Emoji.EuropeanPostOffice => EmojiStrings.EUROPEAN_POST_OFFICE,
        Emoji.Rat => EmojiStrings.RAT,
        Emoji.Mouse => EmojiStrings.MOUSE,
        Emoji.Ox => EmojiStrings.OX,
        Emoji.WaterBuffalo => EmojiStrings.WATER_BUFFALO,
        Emoji.Cow => EmojiStrings.COW,
        Emoji.Tiger => EmojiStrings.TIGER,
        Emoji.Leopard => EmojiStrings.LEOPARD,
        Emoji.Rabbit => EmojiStrings.RABBIT,
        Emoji.Cat => EmojiStrings.CAT,
        Emoji.Dragon => EmojiStrings.DRAGON,
        Emoji.Crocodile => EmojiStrings.CROCODILE,
        Emoji.Whale => EmojiStrings.WHALE,
        Emoji.Ram => EmojiStrings.RAM,
        Emoji.Goat => EmojiStrings.GOAT,
        Emoji.Rooster => EmojiStrings.ROOSTER,
        Emoji.Dog => EmojiStrings.DOG,
        Emoji.Pig => EmojiStrings.PIG,
        Emoji.DromedaryCamel => EmojiStrings.DROMEDARY_CAMEL,
        Emoji.BustsInSilhouette => EmojiStrings.BUSTS_IN_SILHOUETTE,
        Emoji.TwoMenHoldingHands => EmojiStrings.TWO_MEN_HOLDING_HANDS,
        Emoji.TwoWomenHoldingHands => EmojiStrings.TWO_WOMEN_HOLDING_HANDS,
        Emoji.ThoughtBalloon => EmojiStrings.THOUGHT_BALLOON,
        Emoji.BanknoteWithEuroSign => EmojiStrings.BANKNOTE_WITH_EURO_SIGN,
        Emoji.BanknoteWithPoundSign => EmojiStrings.BANKNOTE_WITH_POUND_SIGN,
        Emoji.OpenMailboxWithRaisedFlag => EmojiStrings.OPEN_MAILBOX_WITH_RAISED_FLAG,
        Emoji.OpenMailboxWithLoweredFlag => EmojiStrings.OPEN_MAILBOX_WITH_LOWERED_FLAG,
        Emoji.PostalHorn => EmojiStrings.POSTAL_HORN,
        Emoji.NoMobilePhones => EmojiStrings.NO_MOBILE_PHONES,
        Emoji.TwistedRightwardsArrows => EmojiStrings.TWISTED_RIGHTWARDS_ARROWS,
        Emoji.ClockwiseRightwardsAndLeftwardsOpenCircleArrows => EmojiStrings.CLOCKWISE_RIGHTWARDS_AND_LEFTWARDS_OPEN_CIRCLE_ARROWS,
        Emoji.ClockwiseRightwardsAndLeftwardsOpenCircleArrowsWithCircledOneOverlay => EmojiStrings.CLOCKWISE_RIGHTWARDS_AND_LEFTWARDS_OPEN_CIRCLE_ARROWS_WITH_CIRCLED_ONE_OVERLAY,
        Emoji.AnticlockwiseDownwardsAndUpwardsOpenCircleArrows => EmojiStrings.ANTICLOCKWISE_DOWNWARDS_AND_UPWARDS_OPEN_CIRCLE_ARROWS,
        Emoji.LowBrightnessSymbol => EmojiStrings.LOW_BRIGHTNESS_SYMBOL,
        Emoji.HighBrightnessSymbol => EmojiStrings.HIGH_BRIGHTNESS_SYMBOL,
        Emoji.SpeakerWithCancellationStroke => EmojiStrings.SPEAKER_WITH_CANCELLATION_STROKE,
        Emoji.SpeakerWithOneSoundWave => EmojiStrings.SPEAKER_WITH_ONE_SOUND_WAVE,
        Emoji.BellWithCancellationStroke => EmojiStrings.BELL_WITH_CANCELLATION_STROKE,
        Emoji.Microscope => EmojiStrings.MICROSCOPE,
        Emoji.Telescope => EmojiStrings.TELESCOPE,
        Emoji.ClockFaceOneThirty => EmojiStrings.CLOCK_FACE_ONE_THIRTY,
        Emoji.ClockFaceTwoThirty => EmojiStrings.CLOCK_FACE_TWO_THIRTY,
        Emoji.ClockFaceThreeThirty => EmojiStrings.CLOCK_FACE_THREE_THIRTY,
        Emoji.ClockFaceFourThirty => EmojiStrings.CLOCK_FACE_FOUR_THIRTY,
        Emoji.ClockFaceFiveThirty => EmojiStrings.CLOCK_FACE_FIVE_THIRTY,
        Emoji.ClockFaceSixThirty => EmojiStrings.CLOCK_FACE_SIX_THIRTY,
        Emoji.ClockFaceSevenThirty => EmojiStrings.CLOCK_FACE_SEVEN_THIRTY,
        Emoji.ClockFaceEightThirty => EmojiStrings.CLOCK_FACE_EIGHT_THIRTY,
        Emoji.ClockFaceNineThirty => EmojiStrings.CLOCK_FACE_NINE_THIRTY,
        Emoji.ClockFaceTenThirty => EmojiStrings.CLOCK_FACE_TEN_THIRTY,
        Emoji.ClockFaceElevenThirty => EmojiStrings.CLOCK_FACE_ELEVEN_THIRTY,
        Emoji.ClockFaceTwelveThirty => EmojiStrings.CLOCK_FACE_TWELVE_THIRTY,
        { } => UnknownEmojiSign,
    };

    public static Emoji ToEmoji(this ReadOnlySpan<char> text)
    {
        if (text == EmojiStrings.GRINNING_FACE_WITH_SMILING_EYES) return Emoji.GrinningFaceWithSmilingEyes;
        else if (text == EmojiStrings.FACE_WITH_TEARS_OF_JOY) return Emoji.FaceWithTearsOfJoy;
        else if (text == EmojiStrings.SMILING_FACE_WITH_OPEN_MOUTH) return Emoji.SmilingFaceWithOpenMouth;
        else if (text == EmojiStrings.SMILING_FACE_WITH_OPEN_MOUTH_AND_SMILING_EYES) return Emoji.SmilingFaceWithOpenMouthAndSmilingEyes;
        else if (text == EmojiStrings.SMILING_FACE_WITH_OPEN_MOUTH_AND_COLD_SWEAT) return Emoji.SmilingFaceWithOpenMouthAndColdSweat;
        else if (text == EmojiStrings.SMILING_FACE_WITH_OPEN_MOUTH_AND_TIGHTLY_CLOSED_EYES) return Emoji.SmilingFaceWithOpenMouthAndTightlyClosedEyes;
        else if (text == EmojiStrings.WINKING_FACE) return Emoji.WinkingFace;
        else if (text == EmojiStrings.SMILING_FACE_WITH_SMILING_EYES) return Emoji.SmilingFaceWithSmilingEyes;
        else if (text == EmojiStrings.FACE_SAVOURING_DELICIOUS_FOOD) return Emoji.FaceSavouringDeliciousFood;
        else if (text == EmojiStrings.RELIEVED_FACE) return Emoji.RelievedFace;
        else if (text == EmojiStrings.SMILING_FACE_WITH_HEART_SHAPED_EYES) return Emoji.SmilingFaceWithHeartShapedEyes;
        else if (text == EmojiStrings.SMIRKING_FACE) return Emoji.SmirkingFace;
        else if (text == EmojiStrings.UNAMUSED_FACE) return Emoji.UnamusedFace;
        else if (text == EmojiStrings.FACE_WITH_COLD_SWEAT) return Emoji.FaceWithColdSweat;
        else if (text == EmojiStrings.PENSIVE_FACE) return Emoji.PensiveFace;
        else if (text == EmojiStrings.CONFOUNDED_FACE) return Emoji.ConfoundedFace;
        else if (text == EmojiStrings.FACE_THROWING_A_KISS) return Emoji.FaceThrowingAKiss;
        else if (text == EmojiStrings.KISSING_FACE_WITH_CLOSED_EYES) return Emoji.KissingFaceWithClosedEyes;
        else if (text == EmojiStrings.FACE_WITH_STUCK_OUT_TONGUE_AND_WINKING_EYE) return Emoji.FaceWithStuckOutTongueAndWinkingEye;
        else if (text == EmojiStrings.FACE_WITH_STUCK_OUT_TONGUE_AND_TIGHTLY_CLOSED_EYES) return Emoji.FaceWithStuckOutTongueAndTightlyClosedEyes;
        else if (text == EmojiStrings.DISAPPOINTED_FACE) return Emoji.DisappointedFace;
        else if (text == EmojiStrings.ANGRY_FACE) return Emoji.AngryFace;
        else if (text == EmojiStrings.POUTING_FACE) return Emoji.PoutingFace;
        else if (text == EmojiStrings.CRYING_FACE) return Emoji.CryingFace;
        else if (text == EmojiStrings.PERSEVERING_FACE) return Emoji.PerseveringFace;
        else if (text == EmojiStrings.FACE_WITH_LOOK_OF_TRIUMPH) return Emoji.FaceWithLookOfTriumph;
        else if (text == EmojiStrings.DISAPPOINTED_BUT_RELIEVED_FACE) return Emoji.DisappointedButRelievedFace;
        else if (text == EmojiStrings.FEARFUL_FACE) return Emoji.FearfulFace;
        else if (text == EmojiStrings.WEARY_FACE) return Emoji.WearyFace;
        else if (text == EmojiStrings.SLEEPY_FACE) return Emoji.SleepyFace;
        else if (text == EmojiStrings.TIRED_FACE) return Emoji.TiredFace;
        else if (text == EmojiStrings.LOUDLY_CRYING_FACE) return Emoji.LoudlyCryingFace;
        else if (text == EmojiStrings.FACE_WITH_OPEN_MOUTH_AND_COLD_SWEAT) return Emoji.FaceWithOpenMouthAndColdSweat;
        else if (text == EmojiStrings.FACE_SCREAMING_IN_FEAR) return Emoji.FaceScreamingInFear;
        else if (text == EmojiStrings.ASTONISHED_FACE) return Emoji.AstonishedFace;
        else if (text == EmojiStrings.FLUSHED_FACE) return Emoji.FlushedFace;
        else if (text == EmojiStrings.DIZZY_FACE) return Emoji.DizzyFace;
        else if (text == EmojiStrings.FACE_WITH_MEDICAL_MASK) return Emoji.FaceWithMedicalMask;
        else if (text == EmojiStrings.GRINNING_CAT_FACE_WITH_SMILING_EYES) return Emoji.GrinningCatFaceWithSmilingEyes;
        else if (text == EmojiStrings.CAT_FACE_WITH_TEARS_OF_JOY) return Emoji.CatFaceWithTearsOfJoy;
        else if (text == EmojiStrings.SMILING_CAT_FACE_WITH_OPEN_MOUTH) return Emoji.SmilingCatFaceWithOpenMouth;
        else if (text == EmojiStrings.SMILING_CAT_FACE_WITH_HEART_SHAPED_EYES) return Emoji.SmilingCatFaceWithHeartShapedEyes;
        else if (text == EmojiStrings.CAT_FACE_WITH_WRY_SMILE) return Emoji.CatFaceWithWrySmile;
        else if (text == EmojiStrings.KISSING_CAT_FACE_WITH_CLOSED_EYES) return Emoji.KissingCatFaceWithClosedEyes;
        else if (text == EmojiStrings.POUTING_CAT_FACE) return Emoji.PoutingCatFace;
        else if (text == EmojiStrings.CRYING_CAT_FACE) return Emoji.CryingCatFace;
        else if (text == EmojiStrings.WEARY_CAT_FACE) return Emoji.WearyCatFace;
        else if (text == EmojiStrings.FACE_WITH_NO_GOOD_GESTURE) return Emoji.FaceWithNoGoodGesture;
        else if (text == EmojiStrings.FACE_WITH_OK_GESTURE) return Emoji.FaceWithOkGesture;
        else if (text == EmojiStrings.PERSON_BOWING_DEEPLY) return Emoji.PersonBowingDeeply;
        else if (text == EmojiStrings.SEE_NO_EVIL_MONKEY) return Emoji.SeeNoEvilMonkey;
        else if (text == EmojiStrings.HEAR_NO_EVIL_MONKEY) return Emoji.HearNoEvilMonkey;
        else if (text == EmojiStrings.SPEAK_NO_EVIL_MONKEY) return Emoji.SpeakNoEvilMonkey;
        else if (text == EmojiStrings.HAPPY_PERSON_RAISING_ONE_HAND) return Emoji.HappyPersonRaisingOneHand;
        else if (text == EmojiStrings.PERSON_RAISING_BOTH_HANDS_IN_CELEBRATION) return Emoji.PersonRaisingBothHandsInCelebration;
        else if (text == EmojiStrings.PERSON_FROWNING) return Emoji.PersonFrowning;
        else if (text == EmojiStrings.PERSON_WITH_POUTING_FACE) return Emoji.PersonWithPoutingFace;
        else if (text == EmojiStrings.PERSON_WITH_FOLDED_HANDS) return Emoji.PersonWithFoldedHands;
        else if (text == EmojiStrings.BLACK_SCISSORS) return Emoji.BlackScissors;
        else if (text == EmojiStrings.WHITE_HEAVY_CHECK_MARK) return Emoji.WhiteHeavyCheckMark;
        else if (text == EmojiStrings.AIRPLANE) return Emoji.Airplane;
        else if (text == EmojiStrings.ENVELOPE) return Emoji.Envelope;
        else if (text == EmojiStrings.RAISED_FIST) return Emoji.RaisedFist;
        else if (text == EmojiStrings.RAISED_HAND) return Emoji.RaisedHand;
        else if (text == EmojiStrings.VICTORY_HAND) return Emoji.VictoryHand;
        else if (text == EmojiStrings.PENCIL) return Emoji.Pencil;
        else if (text == EmojiStrings.BLACK_NIB) return Emoji.BlackNib;
        else if (text == EmojiStrings.HEAVY_CHECK_MARK) return Emoji.HeavyCheckMark;
        else if (text == EmojiStrings.HEAVY_MULTIPLICATION_X) return Emoji.HeavyMultiplicationX;
        else if (text == EmojiStrings.SPARKLES) return Emoji.Sparkles;
        else if (text == EmojiStrings.EIGHT_SPOKED_ASTERISK) return Emoji.EightSpokedAsterisk;
        else if (text == EmojiStrings.EIGHT_POINTED_BLACK_STAR) return Emoji.EightPointedBlackStar;
        else if (text == EmojiStrings.SNOWFLAKE) return Emoji.Snowflake;
        else if (text == EmojiStrings.SPARKLE) return Emoji.Sparkle;
        else if (text == EmojiStrings.CROSS_MARK) return Emoji.CrossMark;
        else if (text == EmojiStrings.NEGATIVE_SQUARED_CROSS_MARK) return Emoji.NegativeSquaredCrossMark;
        else if (text == EmojiStrings.BLACK_QUESTION_MARK_ORNAMENT) return Emoji.BlackQuestionMarkOrnament;
        else if (text == EmojiStrings.WHITE_QUESTION_MARK_ORNAMENT) return Emoji.WhiteQuestionMarkOrnament;
        else if (text == EmojiStrings.WHITE_EXCLAMATION_MARK_ORNAMENT) return Emoji.WhiteExclamationMarkOrnament;
        else if (text == EmojiStrings.HEAVY_EXCLAMATION_MARK_SYMBOL) return Emoji.HeavyExclamationMarkSymbol;
        else if (text == EmojiStrings.HEAVY_BLACK_HEART) return Emoji.HeavyBlackHeart;
        else if (text == EmojiStrings.HEAVY_PLUS_SIGN) return Emoji.HeavyPlusSign;
        else if (text == EmojiStrings.HEAVY_MINUS_SIGN) return Emoji.HeavyMinusSign;
        else if (text == EmojiStrings.HEAVY_DIVISION_SIGN) return Emoji.HeavyDivisionSign;
        else if (text == EmojiStrings.BLACK_RIGHTWARDS_ARROW) return Emoji.BlackRightwardsArrow;
        else if (text == EmojiStrings.CURLY_LOOP) return Emoji.CurlyLoop;
        else if (text == EmojiStrings.ROCKET) return Emoji.Rocket;
        else if (text == EmojiStrings.RAILWAY_CAR) return Emoji.RailwayCar;
        else if (text == EmojiStrings.HIGH_SPEED_TRAIN) return Emoji.HighSpeedTrain;
        else if (text == EmojiStrings.HIGH_SPEED_TRAIN_WITH_BULLET_NOSE) return Emoji.HighSpeedTrainWithBulletNose;
        else if (text == EmojiStrings.METRO) return Emoji.Metro;
        else if (text == EmojiStrings.STATION) return Emoji.Station;
        else if (text == EmojiStrings.BUS) return Emoji.Bus;
        else if (text == EmojiStrings.BUS_STOP) return Emoji.BusStop;
        else if (text == EmojiStrings.AMBULANCE) return Emoji.Ambulance;
        else if (text == EmojiStrings.FIRE_ENGINE) return Emoji.FireEngine;
        else if (text == EmojiStrings.POLICE_CAR) return Emoji.PoliceCar;
        else if (text == EmojiStrings.TAXI) return Emoji.Taxi;
        else if (text == EmojiStrings.AUTOMOBILE) return Emoji.Automobile;
        else if (text == EmojiStrings.RECREATIONAL_VEHICLE) return Emoji.RecreationalVehicle;
        else if (text == EmojiStrings.DELIVERY_TRUCK) return Emoji.DeliveryTruck;
        else if (text == EmojiStrings.SHIP) return Emoji.Ship;
        else if (text == EmojiStrings.SPEEDBOAT) return Emoji.Speedboat;
        else if (text == EmojiStrings.HORIZONTAL_TRAFFIC_LIGHT) return Emoji.HorizontalTrafficLight;
        else if (text == EmojiStrings.CONSTRUCTION_SIGN) return Emoji.ConstructionSign;
        else if (text == EmojiStrings.POLICE_CARS_REVOLVING_LIGHT) return Emoji.PoliceCarsRevolvingLight;
        else if (text == EmojiStrings.TRIANGULAR_FLAG_ON_POST) return Emoji.TriangularFlagOnPost;
        else if (text == EmojiStrings.DOOR) return Emoji.Door;
        else if (text == EmojiStrings.NO_ENTRY_SIGN) return Emoji.NoEntrySign;
        else if (text == EmojiStrings.SMOKING_SYMBOL) return Emoji.SmokingSymbol;
        else if (text == EmojiStrings.NO_SMOKING_SYMBOL) return Emoji.NoSmokingSymbol;
        else if (text == EmojiStrings.BICYCLE) return Emoji.Bicycle;
        else if (text == EmojiStrings.PEDESTRIAN) return Emoji.Pedestrian;
        else if (text == EmojiStrings.MENS_SYMBOL) return Emoji.MensSymbol;
        else if (text == EmojiStrings.WOMENS_SYMBOL) return Emoji.WomensSymbol;
        else if (text == EmojiStrings.RESTROOM) return Emoji.Restroom;
        else if (text == EmojiStrings.BABY_SYMBOL) return Emoji.BabySymbol;
        else if (text == EmojiStrings.TOILET) return Emoji.Toilet;
        else if (text == EmojiStrings.WATER_CLOSET) return Emoji.WaterCloset;
        else if (text == EmojiStrings.BATH) return Emoji.Bath;
        else if (text == EmojiStrings.CIRCLED_LATIN_CAPITAL_LETTER_M) return Emoji.CircledLatinCapitalLetterM;
        else if (text == EmojiStrings.NEGATIVE_SQUARED_LATIN_CAPITAL_LETTER_A) return Emoji.NegativeSquaredLatinCapitalLetterA;
        else if (text == EmojiStrings.NEGATIVE_SQUARED_LATIN_CAPITAL_LETTER_B) return Emoji.NegativeSquaredLatinCapitalLetterB;
        else if (text == EmojiStrings.NEGATIVE_SQUARED_LATIN_CAPITAL_LETTER_O) return Emoji.NegativeSquaredLatinCapitalLetterO;
        else if (text == EmojiStrings.NEGATIVE_SQUARED_LATIN_CAPITAL_LETTER_P) return Emoji.NegativeSquaredLatinCapitalLetterP;
        else if (text == EmojiStrings.NEGATIVE_SQUARED_AB) return Emoji.NegativeSquaredAb;
        else if (text == EmojiStrings.SQUARED_CL) return Emoji.SquaredCl;
        else if (text == EmojiStrings.SQUARED_COOL) return Emoji.SquaredCool;
        else if (text == EmojiStrings.SQUARED_FREE) return Emoji.SquaredFree;
        else if (text == EmojiStrings.SQUARED_ID) return Emoji.SquaredId;
        else if (text == EmojiStrings.SQUARED_NEW) return Emoji.SquaredNew;
        else if (text == EmojiStrings.SQUARED_NG) return Emoji.SquaredNg;
        else if (text == EmojiStrings.SQUARED_OK) return Emoji.SquaredOk;
        else if (text == EmojiStrings.SQUARED_SOS) return Emoji.SquaredSos;
        else if (text == EmojiStrings.SQUARED_UP_WITH_EXCLAMATION_MARK) return Emoji.SquaredUpWithExclamationMark;
        else if (text == EmojiStrings.SQUARED_VS) return Emoji.SquaredVs;
        else if (text == EmojiStrings.REGIONAL_INDICATOR_SYMBOL_LETTER_D_REGIONAL_INDICATOR_SYMBOL_LETTER_E) return Emoji.RegionalIndicatorSymbolLetterDRegionalIndicatorSymbolLetterE;
        else if (text == EmojiStrings.REGIONAL_INDICATOR_SYMBOL_LETTER_G_REGIONAL_INDICATOR_SYMBOL_LETTER_B) return Emoji.RegionalIndicatorSymbolLetterGRegionalIndicatorSymbolLetterB;
        else if (text == EmojiStrings.REGIONAL_INDICATOR_SYMBOL_LETTER_C_REGIONAL_INDICATOR_SYMBOL_LETTER_N) return Emoji.RegionalIndicatorSymbolLetterCRegionalIndicatorSymbolLetterN;
        else if (text == EmojiStrings.REGIONAL_INDICATOR_SYMBOL_LETTER_J_REGIONAL_INDICATOR_SYMBOL_LETTER_P) return Emoji.RegionalIndicatorSymbolLetterJRegionalIndicatorSymbolLetterP;
        else if (text == EmojiStrings.REGIONAL_INDICATOR_SYMBOL_LETTER_F_REGIONAL_INDICATOR_SYMBOL_LETTER_R) return Emoji.RegionalIndicatorSymbolLetterFRegionalIndicatorSymbolLetterR;
        else if (text == EmojiStrings.REGIONAL_INDICATOR_SYMBOL_LETTER_K_REGIONAL_INDICATOR_SYMBOL_LETTER_R) return Emoji.RegionalIndicatorSymbolLetterKRegionalIndicatorSymbolLetterR;
        else if (text == EmojiStrings.REGIONAL_INDICATOR_SYMBOL_LETTER_E_REGIONAL_INDICATOR_SYMBOL_LETTER_S) return Emoji.RegionalIndicatorSymbolLetterERegionalIndicatorSymbolLetterS;
        else if (text == EmojiStrings.REGIONAL_INDICATOR_SYMBOL_LETTER_I_REGIONAL_INDICATOR_SYMBOL_LETTER_T) return Emoji.RegionalIndicatorSymbolLetterIRegionalIndicatorSymbolLetterT;
        else if (text == EmojiStrings.REGIONAL_INDICATOR_SYMBOL_LETTER_R_REGIONAL_INDICATOR_SYMBOL_LETTER_U) return Emoji.RegionalIndicatorSymbolLetterRRegionalIndicatorSymbolLetterU;
        else if (text == EmojiStrings.REGIONAL_INDICATOR_SYMBOL_LETTER_U_REGIONAL_INDICATOR_SYMBOL_LETTER_S) return Emoji.RegionalIndicatorSymbolLetterURegionalIndicatorSymbolLetterS;
        else if (text == EmojiStrings.SQUARED_KATAKANA_KOKO) return Emoji.SquaredKatakanaKoko;
        else if (text == EmojiStrings.SQUARED_KATAKANA_SA) return Emoji.SquaredKatakanaSa;
        else if (text == EmojiStrings.SQUARED_CJK_UNIFIED_IDEOGRAPH_7121) return Emoji.SquaredCjkUnifiedIdeograph7121;
        else if (text == EmojiStrings.SQUARED_CJK_UNIFIED_IDEOGRAPH_6307) return Emoji.SquaredCjkUnifiedIdeograph6307;
        else if (text == EmojiStrings.SQUARED_CJK_UNIFIED_IDEOGRAPH_7981) return Emoji.SquaredCjkUnifiedIdeograph7981;
        else if (text == EmojiStrings.SQUARED_CJK_UNIFIED_IDEOGRAPH_7A7A) return Emoji.SquaredCjkUnifiedIdeograph7a7a;
        else if (text == EmojiStrings.SQUARED_CJK_UNIFIED_IDEOGRAPH_5408) return Emoji.SquaredCjkUnifiedIdeograph5408;
        else if (text == EmojiStrings.SQUARED_CJK_UNIFIED_IDEOGRAPH_6E80) return Emoji.SquaredCjkUnifiedIdeograph6e80;
        else if (text == EmojiStrings.SQUARED_CJK_UNIFIED_IDEOGRAPH_6709) return Emoji.SquaredCjkUnifiedIdeograph6709;
        else if (text == EmojiStrings.SQUARED_CJK_UNIFIED_IDEOGRAPH_6708) return Emoji.SquaredCjkUnifiedIdeograph6708;
        else if (text == EmojiStrings.SQUARED_CJK_UNIFIED_IDEOGRAPH_7533) return Emoji.SquaredCjkUnifiedIdeograph7533;
        else if (text == EmojiStrings.SQUARED_CJK_UNIFIED_IDEOGRAPH_5272) return Emoji.SquaredCjkUnifiedIdeograph5272;
        else if (text == EmojiStrings.SQUARED_CJK_UNIFIED_IDEOGRAPH_55B6) return Emoji.SquaredCjkUnifiedIdeograph55b6;
        else if (text == EmojiStrings.CIRCLED_IDEOGRAPH_ADVANTAGE) return Emoji.CircledIdeographAdvantage;
        else if (text == EmojiStrings.CIRCLED_IDEOGRAPH_ACCEPT) return Emoji.CircledIdeographAccept;
        else if (text == EmojiStrings.COPYRIGHT_SIGN) return Emoji.CopyrightSign;
        else if (text == EmojiStrings.REGISTERED_SIGN) return Emoji.RegisteredSign;
        else if (text == EmojiStrings.DOUBLE_EXCLAMATION_MARK) return Emoji.DoubleExclamationMark;
        else if (text == EmojiStrings.EXCLAMATION_QUESTION_MARK) return Emoji.ExclamationQuestionMark;
        else if (text == EmojiStrings.NUMBER_SIGN_COMBINING_ENCLOSING_KEYCAP) return Emoji.NumberSignCombiningEnclosingKeycap;
        else if (text == EmojiStrings.DIGIT_EIGHT_COMBINING_ENCLOSING_KEYCAP) return Emoji.DigitEightCombiningEnclosingKeycap;
        else if (text == EmojiStrings.DIGIT_NINE_COMBINING_ENCLOSING_KEYCAP) return Emoji.DigitNineCombiningEnclosingKeycap;
        else if (text == EmojiStrings.DIGIT_SEVEN_COMBINING_ENCLOSING_KEYCAP) return Emoji.DigitSevenCombiningEnclosingKeycap;
        else if (text == EmojiStrings.DIGIT_ZERO_COMBINING_ENCLOSING_KEYCAP) return Emoji.DigitZeroCombiningEnclosingKeycap;
        else if (text == EmojiStrings.DIGIT_SIX_COMBINING_ENCLOSING_KEYCAP) return Emoji.DigitSixCombiningEnclosingKeycap;
        else if (text == EmojiStrings.DIGIT_FIVE_COMBINING_ENCLOSING_KEYCAP) return Emoji.DigitFiveCombiningEnclosingKeycap;
        else if (text == EmojiStrings.DIGIT_FOUR_COMBINING_ENCLOSING_KEYCAP) return Emoji.DigitFourCombiningEnclosingKeycap;
        else if (text == EmojiStrings.DIGIT_THREE_COMBINING_ENCLOSING_KEYCAP) return Emoji.DigitThreeCombiningEnclosingKeycap;
        else if (text == EmojiStrings.DIGIT_TWO_COMBINING_ENCLOSING_KEYCAP) return Emoji.DigitTwoCombiningEnclosingKeycap;
        else if (text == EmojiStrings.DIGIT_ONE_COMBINING_ENCLOSING_KEYCAP) return Emoji.DigitOneCombiningEnclosingKeycap;
        else if (text == EmojiStrings.TRADE_MARK_SIGN) return Emoji.TradeMarkSign;
        else if (text == EmojiStrings.INFORMATION_SOURCE) return Emoji.InformationSource;
        else if (text == EmojiStrings.LEFT_RIGHT_ARROW) return Emoji.LeftRightArrow;
        else if (text == EmojiStrings.UP_DOWN_ARROW) return Emoji.UpDownArrow;
        else if (text == EmojiStrings.NORTH_WEST_ARROW) return Emoji.NorthWestArrow;
        else if (text == EmojiStrings.NORTH_EAST_ARROW) return Emoji.NorthEastArrow;
        else if (text == EmojiStrings.SOUTH_EAST_ARROW) return Emoji.SouthEastArrow;
        else if (text == EmojiStrings.SOUTH_WEST_ARROW) return Emoji.SouthWestArrow;
        else if (text == EmojiStrings.LEFTWARDS_ARROW_WITH_HOOK) return Emoji.LeftwardsArrowWithHook;
        else if (text == EmojiStrings.RIGHTWARDS_ARROW_WITH_HOOK) return Emoji.RightwardsArrowWithHook;
        else if (text == EmojiStrings.WATCH) return Emoji.Watch;
        else if (text == EmojiStrings.HOURGLASS) return Emoji.Hourglass;
        else if (text == EmojiStrings.BLACK_RIGHT_POINTING_DOUBLE_TRIANGLE) return Emoji.BlackRightPointingDoubleTriangle;
        else if (text == EmojiStrings.BLACK_LEFT_POINTING_DOUBLE_TRIANGLE) return Emoji.BlackLeftPointingDoubleTriangle;
        else if (text == EmojiStrings.BLACK_UP_POINTING_DOUBLE_TRIANGLE) return Emoji.BlackUpPointingDoubleTriangle;
        else if (text == EmojiStrings.BLACK_DOWN_POINTING_DOUBLE_TRIANGLE) return Emoji.BlackDownPointingDoubleTriangle;
        else if (text == EmojiStrings.ALARM_CLOCK) return Emoji.AlarmClock;
        else if (text == EmojiStrings.HOURGLASS_WITH_FLOWING_SAND) return Emoji.HourglassWithFlowingSand;
        else if (text == EmojiStrings.BLACK_SMALL_SQUARE) return Emoji.BlackSmallSquare;
        else if (text == EmojiStrings.WHITE_SMALL_SQUARE) return Emoji.WhiteSmallSquare;
        else if (text == EmojiStrings.BLACK_RIGHT_POINTING_TRIANGLE) return Emoji.BlackRightPointingTriangle;
        else if (text == EmojiStrings.BLACK_LEFT_POINTING_TRIANGLE) return Emoji.BlackLeftPointingTriangle;
        else if (text == EmojiStrings.WHITE_MEDIUM_SQUARE) return Emoji.WhiteMediumSquare;
        else if (text == EmojiStrings.BLACK_MEDIUM_SQUARE) return Emoji.BlackMediumSquare;
        else if (text == EmojiStrings.WHITE_MEDIUM_SMALL_SQUARE) return Emoji.WhiteMediumSmallSquare;
        else if (text == EmojiStrings.BLACK_MEDIUM_SMALL_SQUARE) return Emoji.BlackMediumSmallSquare;
        else if (text == EmojiStrings.BLACK_SUN_WITH_RAYS) return Emoji.BlackSunWithRays;
        else if (text == EmojiStrings.CLOUD) return Emoji.Cloud;
        else if (text == EmojiStrings.BLACK_TELEPHONE) return Emoji.BlackTelephone;
        else if (text == EmojiStrings.BALLOT_BOX_WITH_CHECK) return Emoji.BallotBoxWithCheck;
        else if (text == EmojiStrings.UMBRELLA_WITH_RAIN_DROPS) return Emoji.UmbrellaWithRainDrops;
        else if (text == EmojiStrings.HOT_BEVERAGE) return Emoji.HotBeverage;
        else if (text == EmojiStrings.WHITE_UP_POINTING_INDEX) return Emoji.WhiteUpPointingIndex;
        else if (text == EmojiStrings.WHITE_SMILING_FACE) return Emoji.WhiteSmilingFace;
        else if (text == EmojiStrings.ARIES) return Emoji.Aries;
        else if (text == EmojiStrings.TAURUS) return Emoji.Taurus;
        else if (text == EmojiStrings.GEMINI) return Emoji.Gemini;
        else if (text == EmojiStrings.CANCER) return Emoji.Cancer;
        else if (text == EmojiStrings.LEO) return Emoji.Leo;
        else if (text == EmojiStrings.VIRGO) return Emoji.Virgo;
        else if (text == EmojiStrings.LIBRA) return Emoji.Libra;
        else if (text == EmojiStrings.SCORPIUS) return Emoji.Scorpius;
        else if (text == EmojiStrings.SAGITTARIUS) return Emoji.Sagittarius;
        else if (text == EmojiStrings.CAPRICORN) return Emoji.Capricorn;
        else if (text == EmojiStrings.AQUARIUS) return Emoji.Aquarius;
        else if (text == EmojiStrings.PISCES) return Emoji.Pisces;
        else if (text == EmojiStrings.BLACK_SPADE_SUIT) return Emoji.BlackSpadeSuit;
        else if (text == EmojiStrings.BLACK_CLUB_SUIT) return Emoji.BlackClubSuit;
        else if (text == EmojiStrings.BLACK_HEART_SUIT) return Emoji.BlackHeartSuit;
        else if (text == EmojiStrings.BLACK_DIAMOND_SUIT) return Emoji.BlackDiamondSuit;
        else if (text == EmojiStrings.HOT_SPRINGS) return Emoji.HotSprings;
        else if (text == EmojiStrings.BLACK_UNIVERSAL_RECYCLING_SYMBOL) return Emoji.BlackUniversalRecyclingSymbol;
        else if (text == EmojiStrings.WHEELCHAIR_SYMBOL) return Emoji.WheelchairSymbol;
        else if (text == EmojiStrings.ANCHOR) return Emoji.Anchor;
        else if (text == EmojiStrings.WARNING_SIGN) return Emoji.WarningSign;
        else if (text == EmojiStrings.HIGH_VOLTAGE_SIGN) return Emoji.HighVoltageSign;
        else if (text == EmojiStrings.MEDIUM_WHITE_CIRCLE) return Emoji.MediumWhiteCircle;
        else if (text == EmojiStrings.MEDIUM_BLACK_CIRCLE) return Emoji.MediumBlackCircle;
        else if (text == EmojiStrings.SOCCER_BALL) return Emoji.SoccerBall;
        else if (text == EmojiStrings.BASEBALL) return Emoji.Baseball;
        else if (text == EmojiStrings.SNOWMAN_WITHOUT_SNOW) return Emoji.SnowmanWithoutSnow;
        else if (text == EmojiStrings.SUN_BEHIND_CLOUD) return Emoji.SunBehindCloud;
        else if (text == EmojiStrings.OPHIUCHUS) return Emoji.Ophiuchus;
        else if (text == EmojiStrings.NO_ENTRY) return Emoji.NoEntry;
        else if (text == EmojiStrings.CHURCH) return Emoji.Church;
        else if (text == EmojiStrings.FOUNTAIN) return Emoji.Fountain;
        else if (text == EmojiStrings.FLAG_IN_HOLE) return Emoji.FlagInHole;
        else if (text == EmojiStrings.SAILBOAT) return Emoji.Sailboat;
        else if (text == EmojiStrings.TENT) return Emoji.Tent;
        else if (text == EmojiStrings.FUEL_PUMP) return Emoji.FuelPump;
        else if (text == EmojiStrings.ARROW_POINTING_RIGHTWARDS_THEN_CURVING_UPWARDS) return Emoji.ArrowPointingRightwardsThenCurvingUpwards;
        else if (text == EmojiStrings.ARROW_POINTING_RIGHTWARDS_THEN_CURVING_DOWNWARDS) return Emoji.ArrowPointingRightwardsThenCurvingDownwards;
        else if (text == EmojiStrings.LEFTWARDS_BLACK_ARROW) return Emoji.LeftwardsBlackArrow;
        else if (text == EmojiStrings.UPWARDS_BLACK_ARROW) return Emoji.UpwardsBlackArrow;
        else if (text == EmojiStrings.DOWNWARDS_BLACK_ARROW) return Emoji.DownwardsBlackArrow;
        else if (text == EmojiStrings.BLACK_LARGE_SQUARE) return Emoji.BlackLargeSquare;
        else if (text == EmojiStrings.WHITE_LARGE_SQUARE) return Emoji.WhiteLargeSquare;
        else if (text == EmojiStrings.WHITE_MEDIUM_STAR) return Emoji.WhiteMediumStar;
        else if (text == EmojiStrings.HEAVY_LARGE_CIRCLE) return Emoji.HeavyLargeCircle;
        else if (text == EmojiStrings.WAVY_DASH) return Emoji.WavyDash;
        else if (text == EmojiStrings.PART_ALTERNATION_MARK) return Emoji.PartAlternationMark;
        else if (text == EmojiStrings.CIRCLED_IDEOGRAPH_CONGRATULATION) return Emoji.CircledIdeographCongratulation;
        else if (text == EmojiStrings.CIRCLED_IDEOGRAPH_SECRET) return Emoji.CircledIdeographSecret;
        else if (text == EmojiStrings.MAHJONG_TILE_RED_DRAGON) return Emoji.MahjongTileRedDragon;
        else if (text == EmojiStrings.PLAYING_CARD_BLACK_JOKER) return Emoji.PlayingCardBlackJoker;
        else if (text == EmojiStrings.CYCLONE) return Emoji.Cyclone;
        else if (text == EmojiStrings.FOGGY) return Emoji.Foggy;
        else if (text == EmojiStrings.CLOSED_UMBRELLA) return Emoji.ClosedUmbrella;
        else if (text == EmojiStrings.NIGHT_WITH_STARS) return Emoji.NightWithStars;
        else if (text == EmojiStrings.SUNRISE_OVER_MOUNTAINS) return Emoji.SunriseOverMountains;
        else if (text == EmojiStrings.SUNRISE) return Emoji.Sunrise;
        else if (text == EmojiStrings.CITYSCAPE_AT_DUSK) return Emoji.CityscapeAtDusk;
        else if (text == EmojiStrings.SUNSET_OVER_BUILDINGS) return Emoji.SunsetOverBuildings;
        else if (text == EmojiStrings.RAINBOW) return Emoji.Rainbow;
        else if (text == EmojiStrings.BRIDGE_AT_NIGHT) return Emoji.BridgeAtNight;
        else if (text == EmojiStrings.WATER_WAVE) return Emoji.WaterWave;
        else if (text == EmojiStrings.VOLCANO) return Emoji.Volcano;
        else if (text == EmojiStrings.MILKY_WAY) return Emoji.MilkyWay;
        else if (text == EmojiStrings.EARTH_GLOBE_ASIA_AUSTRALIA) return Emoji.EarthGlobeAsiaAustralia;
        else if (text == EmojiStrings.NEW_MOON_SYMBOL) return Emoji.NewMoonSymbol;
        else if (text == EmojiStrings.FIRST_QUARTER_MOON_SYMBOL) return Emoji.FirstQuarterMoonSymbol;
        else if (text == EmojiStrings.WAXING_GIBBOUS_MOON_SYMBOL) return Emoji.WaxingGibbousMoonSymbol;
        else if (text == EmojiStrings.FULL_MOON_SYMBOL) return Emoji.FullMoonSymbol;
        else if (text == EmojiStrings.CRESCENT_MOON) return Emoji.CrescentMoon;
        else if (text == EmojiStrings.FIRST_QUARTER_MOON_WITH_FACE) return Emoji.FirstQuarterMoonWithFace;
        else if (text == EmojiStrings.GLOWING_STAR) return Emoji.GlowingStar;
        else if (text == EmojiStrings.SHOOTING_STAR) return Emoji.ShootingStar;
        else if (text == EmojiStrings.CHESTNUT) return Emoji.Chestnut;
        else if (text == EmojiStrings.SEEDLING) return Emoji.Seedling;
        else if (text == EmojiStrings.PALM_TREE) return Emoji.PalmTree;
        else if (text == EmojiStrings.CACTUS) return Emoji.Cactus;
        else if (text == EmojiStrings.TULIP) return Emoji.Tulip;
        else if (text == EmojiStrings.CHERRY_BLOSSOM) return Emoji.CherryBlossom;
        else if (text == EmojiStrings.ROSE) return Emoji.Rose;
        else if (text == EmojiStrings.HIBISCUS) return Emoji.Hibiscus;
        else if (text == EmojiStrings.SUNFLOWER) return Emoji.Sunflower;
        else if (text == EmojiStrings.BLOSSOM) return Emoji.Blossom;
        else if (text == EmojiStrings.EAR_OF_MAIZE) return Emoji.EarOfMaize;
        else if (text == EmojiStrings.EAR_OF_RICE) return Emoji.EarOfRice;
        else if (text == EmojiStrings.HERB) return Emoji.Herb;
        else if (text == EmojiStrings.FOUR_LEAF_CLOVER) return Emoji.FourLeafClover;
        else if (text == EmojiStrings.MAPLE_LEAF) return Emoji.MapleLeaf;
        else if (text == EmojiStrings.FALLEN_LEAF) return Emoji.FallenLeaf;
        else if (text == EmojiStrings.LEAF_FLUTTERING_IN_WIND) return Emoji.LeafFlutteringInWind;
        else if (text == EmojiStrings.MUSHROOM) return Emoji.Mushroom;
        else if (text == EmojiStrings.TOMATO) return Emoji.Tomato;
        else if (text == EmojiStrings.AUBERGINE) return Emoji.Aubergine;
        else if (text == EmojiStrings.GRAPES) return Emoji.Grapes;
        else if (text == EmojiStrings.MELON) return Emoji.Melon;
        else if (text == EmojiStrings.WATERMELON) return Emoji.Watermelon;
        else if (text == EmojiStrings.TANGERINE) return Emoji.Tangerine;
        else if (text == EmojiStrings.BANANA) return Emoji.Banana;
        else if (text == EmojiStrings.PINEAPPLE) return Emoji.Pineapple;
        else if (text == EmojiStrings.RED_APPLE) return Emoji.RedApple;
        else if (text == EmojiStrings.GREEN_APPLE) return Emoji.GreenApple;
        else if (text == EmojiStrings.PEACH) return Emoji.Peach;
        else if (text == EmojiStrings.CHERRIES) return Emoji.Cherries;
        else if (text == EmojiStrings.STRAWBERRY) return Emoji.Strawberry;
        else if (text == EmojiStrings.HAMBURGER) return Emoji.Hamburger;
        else if (text == EmojiStrings.SLICE_OF_PIZZA) return Emoji.SliceOfPizza;
        else if (text == EmojiStrings.MEAT_ON_BONE) return Emoji.MeatOnBone;
        else if (text == EmojiStrings.POULTRY_LEG) return Emoji.PoultryLeg;
        else if (text == EmojiStrings.RICE_CRACKER) return Emoji.RiceCracker;
        else if (text == EmojiStrings.RICE_BALL) return Emoji.RiceBall;
        else if (text == EmojiStrings.COOKED_RICE) return Emoji.CookedRice;
        else if (text == EmojiStrings.CURRY_AND_RICE) return Emoji.CurryAndRice;
        else if (text == EmojiStrings.STEAMING_BOWL) return Emoji.SteamingBowl;
        else if (text == EmojiStrings.SPAGHETTI) return Emoji.Spaghetti;
        else if (text == EmojiStrings.BREAD) return Emoji.Bread;
        else if (text == EmojiStrings.FRENCH_FRIES) return Emoji.FrenchFries;
        else if (text == EmojiStrings.ROASTED_SWEET_POTATO) return Emoji.RoastedSweetPotato;
        else if (text == EmojiStrings.DANGO) return Emoji.Dango;
        else if (text == EmojiStrings.ODEN) return Emoji.Oden;
        else if (text == EmojiStrings.SUSHI) return Emoji.Sushi;
        else if (text == EmojiStrings.FRIED_SHRIMP) return Emoji.FriedShrimp;
        else if (text == EmojiStrings.FISH_CAKE_WITH_SWIRL_DESIGN) return Emoji.FishCakeWithSwirlDesign;
        else if (text == EmojiStrings.SOFT_ICE_CREAM) return Emoji.SoftIceCream;
        else if (text == EmojiStrings.SHAVED_ICE) return Emoji.ShavedIce;
        else if (text == EmojiStrings.ICE_CREAM) return Emoji.IceCream;
        else if (text == EmojiStrings.DOUGHNUT) return Emoji.Doughnut;
        else if (text == EmojiStrings.COOKIE) return Emoji.Cookie;
        else if (text == EmojiStrings.CHOCOLATE_BAR) return Emoji.ChocolateBar;
        else if (text == EmojiStrings.CANDY) return Emoji.Candy;
        else if (text == EmojiStrings.LOLLIPOP) return Emoji.Lollipop;
        else if (text == EmojiStrings.CUSTARD) return Emoji.Custard;
        else if (text == EmojiStrings.HONEY_POT) return Emoji.HoneyPot;
        else if (text == EmojiStrings.SHORTCAKE) return Emoji.Shortcake;
        else if (text == EmojiStrings.BENTO_BOX) return Emoji.BentoBox;
        else if (text == EmojiStrings.POT_OF_FOOD) return Emoji.PotOfFood;
        else if (text == EmojiStrings.COOKING) return Emoji.Cooking;
        else if (text == EmojiStrings.FORK_AND_KNIFE) return Emoji.ForkAndKnife;
        else if (text == EmojiStrings.TEACUP_WITHOUT_HANDLE) return Emoji.TeacupWithoutHandle;
        else if (text == EmojiStrings.SAKE_BOTTLE_AND_CUP) return Emoji.SakeBottleAndCup;
        else if (text == EmojiStrings.WINE_GLASS) return Emoji.WineGlass;
        else if (text == EmojiStrings.COCKTAIL_GLASS) return Emoji.CocktailGlass;
        else if (text == EmojiStrings.TROPICAL_DRINK) return Emoji.TropicalDrink;
        else if (text == EmojiStrings.BEER_MUG) return Emoji.BeerMug;
        else if (text == EmojiStrings.CLINKING_BEER_MUGS) return Emoji.ClinkingBeerMugs;
        else if (text == EmojiStrings.RIBBON) return Emoji.Ribbon;
        else if (text == EmojiStrings.WRAPPED_PRESENT) return Emoji.WrappedPresent;
        else if (text == EmojiStrings.BIRTHDAY_CAKE) return Emoji.BirthdayCake;
        else if (text == EmojiStrings.JACK_O_LANTERN) return Emoji.JackOLantern;
        else if (text == EmojiStrings.CHRISTMAS_TREE) return Emoji.ChristmasTree;
        else if (text == EmojiStrings.FATHER_CHRISTMAS) return Emoji.FatherChristmas;
        else if (text == EmojiStrings.FIREWORKS) return Emoji.Fireworks;
        else if (text == EmojiStrings.FIREWORK_SPARKLER) return Emoji.FireworkSparkler;
        else if (text == EmojiStrings.BALLOON) return Emoji.Balloon;
        else if (text == EmojiStrings.PARTY_POPPER) return Emoji.PartyPopper;
        else if (text == EmojiStrings.CONFETTI_BALL) return Emoji.ConfettiBall;
        else if (text == EmojiStrings.TANABATA_TREE) return Emoji.TanabataTree;
        else if (text == EmojiStrings.CROSSED_FLAGS) return Emoji.CrossedFlags;
        else if (text == EmojiStrings.PINE_DECORATION) return Emoji.PineDecoration;
        else if (text == EmojiStrings.JAPANESE_DOLLS) return Emoji.JapaneseDolls;
        else if (text == EmojiStrings.CARP_STREAMER) return Emoji.CarpStreamer;
        else if (text == EmojiStrings.WIND_CHIME) return Emoji.WindChime;
        else if (text == EmojiStrings.MOON_VIEWING_CEREMONY) return Emoji.MoonViewingCeremony;
        else if (text == EmojiStrings.SCHOOL_SATCHEL) return Emoji.SchoolSatchel;
        else if (text == EmojiStrings.GRADUATION_CAP) return Emoji.GraduationCap;
        else if (text == EmojiStrings.CAROUSEL_HORSE) return Emoji.CarouselHorse;
        else if (text == EmojiStrings.FERRIS_WHEEL) return Emoji.FerrisWheel;
        else if (text == EmojiStrings.ROLLER_COASTER) return Emoji.RollerCoaster;
        else if (text == EmojiStrings.FISHING_POLE_AND_FISH) return Emoji.FishingPoleAndFish;
        else if (text == EmojiStrings.MICROPHONE) return Emoji.Microphone;
        else if (text == EmojiStrings.MOVIE_CAMERA) return Emoji.MovieCamera;
        else if (text == EmojiStrings.CINEMA) return Emoji.Cinema;
        else if (text == EmojiStrings.HEADPHONE) return Emoji.Headphone;
        else if (text == EmojiStrings.ARTIST_PALETTE) return Emoji.ArtistPalette;
        else if (text == EmojiStrings.TOP_HAT) return Emoji.TopHat;
        else if (text == EmojiStrings.CIRCUS_TENT) return Emoji.CircusTent;
        else if (text == EmojiStrings.TICKET) return Emoji.Ticket;
        else if (text == EmojiStrings.CLAPPER_BOARD) return Emoji.ClapperBoard;
        else if (text == EmojiStrings.PERFORMING_ARTS) return Emoji.PerformingArts;
        else if (text == EmojiStrings.VIDEO_GAME) return Emoji.VideoGame;
        else if (text == EmojiStrings.DIRECT_HIT) return Emoji.DirectHit;
        else if (text == EmojiStrings.SLOT_MACHINE) return Emoji.SlotMachine;
        else if (text == EmojiStrings.BILLIARDS) return Emoji.Billiards;
        else if (text == EmojiStrings.GAME_DIE) return Emoji.GameDie;
        else if (text == EmojiStrings.BOWLING) return Emoji.Bowling;
        else if (text == EmojiStrings.FLOWER_PLAYING_CARDS) return Emoji.FlowerPlayingCards;
        else if (text == EmojiStrings.MUSICAL_NOTE) return Emoji.MusicalNote;
        else if (text == EmojiStrings.MULTIPLE_MUSICAL_NOTES) return Emoji.MultipleMusicalNotes;
        else if (text == EmojiStrings.SAXOPHONE) return Emoji.Saxophone;
        else if (text == EmojiStrings.GUITAR) return Emoji.Guitar;
        else if (text == EmojiStrings.MUSICAL_KEYBOARD) return Emoji.MusicalKeyboard;
        else if (text == EmojiStrings.TRUMPET) return Emoji.Trumpet;
        else if (text == EmojiStrings.VIOLIN) return Emoji.Violin;
        else if (text == EmojiStrings.MUSICAL_SCORE) return Emoji.MusicalScore;
        else if (text == EmojiStrings.RUNNING_SHIRT_WITH_SASH) return Emoji.RunningShirtWithSash;
        else if (text == EmojiStrings.TENNIS_RACQUET_AND_BALL) return Emoji.TennisRacquetAndBall;
        else if (text == EmojiStrings.SKI_AND_SKI_BOOT) return Emoji.SkiAndSkiBoot;
        else if (text == EmojiStrings.BASKETBALL_AND_HOOP) return Emoji.BasketballAndHoop;
        else if (text == EmojiStrings.CHEQUERED_FLAG) return Emoji.ChequeredFlag;
        else if (text == EmojiStrings.SNOWBOARDER) return Emoji.Snowboarder;
        else if (text == EmojiStrings.RUNNER) return Emoji.Runner;
        else if (text == EmojiStrings.SURFER) return Emoji.Surfer;
        else if (text == EmojiStrings.TROPHY) return Emoji.Trophy;
        else if (text == EmojiStrings.AMERICAN_FOOTBALL) return Emoji.AmericanFootball;
        else if (text == EmojiStrings.SWIMMER) return Emoji.Swimmer;
        else if (text == EmojiStrings.HOUSE_BUILDING) return Emoji.HouseBuilding;
        else if (text == EmojiStrings.HOUSE_WITH_GARDEN) return Emoji.HouseWithGarden;
        else if (text == EmojiStrings.OFFICE_BUILDING) return Emoji.OfficeBuilding;
        else if (text == EmojiStrings.JAPANESE_POST_OFFICE) return Emoji.JapanesePostOffice;
        else if (text == EmojiStrings.HOSPITAL) return Emoji.Hospital;
        else if (text == EmojiStrings.BANK) return Emoji.Bank;
        else if (text == EmojiStrings.AUTOMATED_TELLER_MACHINE) return Emoji.AutomatedTellerMachine;
        else if (text == EmojiStrings.HOTEL) return Emoji.Hotel;
        else if (text == EmojiStrings.LOVE_HOTEL) return Emoji.LoveHotel;
        else if (text == EmojiStrings.CONVENIENCE_STORE) return Emoji.ConvenienceStore;
        else if (text == EmojiStrings.SCHOOL) return Emoji.School;
        else if (text == EmojiStrings.DEPARTMENT_STORE) return Emoji.DepartmentStore;
        else if (text == EmojiStrings.FACTORY) return Emoji.Factory;
        else if (text == EmojiStrings.IZAKAYA_LANTERN) return Emoji.IzakayaLantern;
        else if (text == EmojiStrings.JAPANESE_CASTLE) return Emoji.JapaneseCastle;
        else if (text == EmojiStrings.EUROPEAN_CASTLE) return Emoji.EuropeanCastle;
        else if (text == EmojiStrings.SNAIL) return Emoji.Snail;
        else if (text == EmojiStrings.SNAKE) return Emoji.Snake;
        else if (text == EmojiStrings.HORSE) return Emoji.Horse;
        else if (text == EmojiStrings.SHEEP) return Emoji.Sheep;
        else if (text == EmojiStrings.MONKEY) return Emoji.Monkey;
        else if (text == EmojiStrings.CHICKEN) return Emoji.Chicken;
        else if (text == EmojiStrings.BOAR) return Emoji.Boar;
        else if (text == EmojiStrings.ELEPHANT) return Emoji.Elephant;
        else if (text == EmojiStrings.OCTOPUS) return Emoji.Octopus;
        else if (text == EmojiStrings.SPIRAL_SHELL) return Emoji.SpiralShell;
        else if (text == EmojiStrings.BUG) return Emoji.Bug;
        else if (text == EmojiStrings.ANT) return Emoji.Ant;
        else if (text == EmojiStrings.HONEYBEE) return Emoji.Honeybee;
        else if (text == EmojiStrings.LADY_BEETLE) return Emoji.LadyBeetle;
        else if (text == EmojiStrings.FISH) return Emoji.Fish;
        else if (text == EmojiStrings.TROPICAL_FISH) return Emoji.TropicalFish;
        else if (text == EmojiStrings.BLOWFISH) return Emoji.Blowfish;
        else if (text == EmojiStrings.TURTLE) return Emoji.Turtle;
        else if (text == EmojiStrings.HATCHING_CHICK) return Emoji.HatchingChick;
        else if (text == EmojiStrings.BABY_CHICK) return Emoji.BabyChick;
        else if (text == EmojiStrings.FRONT_FACING_BABY_CHICK) return Emoji.FrontFacingBabyChick;
        else if (text == EmojiStrings.BIRD) return Emoji.Bird;
        else if (text == EmojiStrings.PENGUIN) return Emoji.Penguin;
        else if (text == EmojiStrings.KOALA) return Emoji.Koala;
        else if (text == EmojiStrings.POODLE) return Emoji.Poodle;
        else if (text == EmojiStrings.BACTRIAN_CAMEL) return Emoji.BactrianCamel;
        else if (text == EmojiStrings.DOLPHIN) return Emoji.Dolphin;
        else if (text == EmojiStrings.MOUSE_FACE) return Emoji.MouseFace;
        else if (text == EmojiStrings.COW_FACE) return Emoji.CowFace;
        else if (text == EmojiStrings.TIGER_FACE) return Emoji.TigerFace;
        else if (text == EmojiStrings.RABBIT_FACE) return Emoji.RabbitFace;
        else if (text == EmojiStrings.CAT_FACE) return Emoji.CatFace;
        else if (text == EmojiStrings.DRAGON_FACE) return Emoji.DragonFace;
        else if (text == EmojiStrings.SPOUTING_WHALE) return Emoji.SpoutingWhale;
        else if (text == EmojiStrings.HORSE_FACE) return Emoji.HorseFace;
        else if (text == EmojiStrings.MONKEY_FACE) return Emoji.MonkeyFace;
        else if (text == EmojiStrings.DOG_FACE) return Emoji.DogFace;
        else if (text == EmojiStrings.PIG_FACE) return Emoji.PigFace;
        else if (text == EmojiStrings.FROG_FACE) return Emoji.FrogFace;
        else if (text == EmojiStrings.HAMSTER_FACE) return Emoji.HamsterFace;
        else if (text == EmojiStrings.WOLF_FACE) return Emoji.WolfFace;
        else if (text == EmojiStrings.BEAR_FACE) return Emoji.BearFace;
        else if (text == EmojiStrings.PANDA_FACE) return Emoji.PandaFace;
        else if (text == EmojiStrings.PIG_NOSE) return Emoji.PigNose;
        else if (text == EmojiStrings.PAW_PRINTS) return Emoji.PawPrints;
        else if (text == EmojiStrings.EYES) return Emoji.Eyes;
        else if (text == EmojiStrings.EAR) return Emoji.Ear;
        else if (text == EmojiStrings.NOSE) return Emoji.Nose;
        else if (text == EmojiStrings.MOUTH) return Emoji.Mouth;
        else if (text == EmojiStrings.TONGUE) return Emoji.Tongue;
        else if (text == EmojiStrings.WHITE_UP_POINTING_BACKHAND_INDEX) return Emoji.WhiteUpPointingBackhandIndex;
        else if (text == EmojiStrings.WHITE_DOWN_POINTING_BACKHAND_INDEX) return Emoji.WhiteDownPointingBackhandIndex;
        else if (text == EmojiStrings.WHITE_LEFT_POINTING_BACKHAND_INDEX) return Emoji.WhiteLeftPointingBackhandIndex;
        else if (text == EmojiStrings.WHITE_RIGHT_POINTING_BACKHAND_INDEX) return Emoji.WhiteRightPointingBackhandIndex;
        else if (text == EmojiStrings.FISTED_HAND_SIGN) return Emoji.FistedHandSign;
        else if (text == EmojiStrings.WAVING_HAND_SIGN) return Emoji.WavingHandSign;
        else if (text == EmojiStrings.OK_HAND_SIGN) return Emoji.OkHandSign;
        else if (text == EmojiStrings.THUMBS_UP_SIGN) return Emoji.ThumbsUpSign;
        else if (text == EmojiStrings.THUMBS_DOWN_SIGN) return Emoji.ThumbsDownSign;
        else if (text == EmojiStrings.CLAPPING_HANDS_SIGN) return Emoji.ClappingHandsSign;
        else if (text == EmojiStrings.OPEN_HANDS_SIGN) return Emoji.OpenHandsSign;
        else if (text == EmojiStrings.CROWN) return Emoji.Crown;
        else if (text == EmojiStrings.WOMANS_HAT) return Emoji.WomansHat;
        else if (text == EmojiStrings.EYEGLASSES) return Emoji.Eyeglasses;
        else if (text == EmojiStrings.NECKTIE) return Emoji.Necktie;
        else if (text == EmojiStrings.T_SHIRT) return Emoji.TShirt;
        else if (text == EmojiStrings.JEANS) return Emoji.Jeans;
        else if (text == EmojiStrings.DRESS) return Emoji.Dress;
        else if (text == EmojiStrings.KIMONO) return Emoji.Kimono;
        else if (text == EmojiStrings.BIKINI) return Emoji.Bikini;
        else if (text == EmojiStrings.WOMANS_CLOTHES) return Emoji.WomansClothes;
        else if (text == EmojiStrings.PURSE) return Emoji.Purse;
        else if (text == EmojiStrings.HANDBAG) return Emoji.Handbag;
        else if (text == EmojiStrings.POUCH) return Emoji.Pouch;
        else if (text == EmojiStrings.MANS_SHOE) return Emoji.MansShoe;
        else if (text == EmojiStrings.ATHLETIC_SHOE) return Emoji.AthleticShoe;
        else if (text == EmojiStrings.HIGH_HEELED_SHOE) return Emoji.HighHeeledShoe;
        else if (text == EmojiStrings.WOMANS_SANDAL) return Emoji.WomansSandal;
        else if (text == EmojiStrings.WOMANS_BOOTS) return Emoji.WomansBoots;
        else if (text == EmojiStrings.FOOTPRINTS) return Emoji.Footprints;
        else if (text == EmojiStrings.BUST_IN_SILHOUETTE) return Emoji.BustInSilhouette;
        else if (text == EmojiStrings.BOY) return Emoji.Boy;
        else if (text == EmojiStrings.GIRL) return Emoji.Girl;
        else if (text == EmojiStrings.MAN) return Emoji.Man;
        else if (text == EmojiStrings.WOMAN) return Emoji.Woman;
        else if (text == EmojiStrings.FAMILY) return Emoji.Family;
        else if (text == EmojiStrings.MAN_AND_WOMAN_HOLDING_HANDS) return Emoji.ManAndWomanHoldingHands;
        else if (text == EmojiStrings.POLICE_OFFICER) return Emoji.PoliceOfficer;
        else if (text == EmojiStrings.WOMAN_WITH_BUNNY_EARS) return Emoji.WomanWithBunnyEars;
        else if (text == EmojiStrings.BRIDE_WITH_VEIL) return Emoji.BrideWithVeil;
        else if (text == EmojiStrings.PERSON_WITH_BLOND_HAIR) return Emoji.PersonWithBlondHair;
        else if (text == EmojiStrings.MAN_WITH_GUA_PI_MAO) return Emoji.ManWithGuaPiMao;
        else if (text == EmojiStrings.MAN_WITH_TURBAN) return Emoji.ManWithTurban;
        else if (text == EmojiStrings.OLDER_MAN) return Emoji.OlderMan;
        else if (text == EmojiStrings.OLDER_WOMAN) return Emoji.OlderWoman;
        else if (text == EmojiStrings.BABY) return Emoji.Baby;
        else if (text == EmojiStrings.CONSTRUCTION_WORKER) return Emoji.ConstructionWorker;
        else if (text == EmojiStrings.PRINCESS) return Emoji.Princess;
        else if (text == EmojiStrings.JAPANESE_OGRE) return Emoji.JapaneseOgre;
        else if (text == EmojiStrings.JAPANESE_GOBLIN) return Emoji.JapaneseGoblin;
        else if (text == EmojiStrings.GHOST) return Emoji.Ghost;
        else if (text == EmojiStrings.BABY_ANGEL) return Emoji.BabyAngel;
        else if (text == EmojiStrings.EXTRATERRESTRIAL_ALIEN) return Emoji.ExtraterrestrialAlien;
        else if (text == EmojiStrings.ALIEN_MONSTER) return Emoji.AlienMonster;
        else if (text == EmojiStrings.IMP) return Emoji.Imp;
        else if (text == EmojiStrings.SKULL) return Emoji.Skull;
        else if (text == EmojiStrings.INFORMATION_DESK_PERSON) return Emoji.InformationDeskPerson;
        else if (text == EmojiStrings.GUARDSMAN) return Emoji.Guardsman;
        else if (text == EmojiStrings.DANCER) return Emoji.Dancer;
        else if (text == EmojiStrings.LIPSTICK) return Emoji.Lipstick;
        else if (text == EmojiStrings.NAIL_POLISH) return Emoji.NailPolish;
        else if (text == EmojiStrings.FACE_MASSAGE) return Emoji.FaceMassage;
        else if (text == EmojiStrings.HAIRCUT) return Emoji.Haircut;
        else if (text == EmojiStrings.BARBER_POLE) return Emoji.BarberPole;
        else if (text == EmojiStrings.SYRINGE) return Emoji.Syringe;
        else if (text == EmojiStrings.PILL) return Emoji.Pill;
        else if (text == EmojiStrings.KISS_MARK) return Emoji.KissMark;
        else if (text == EmojiStrings.LOVE_LETTER) return Emoji.LoveLetter;
        else if (text == EmojiStrings.RING) return Emoji.Ring;
        else if (text == EmojiStrings.GEM_STONE) return Emoji.GemStone;
        else if (text == EmojiStrings.KISS) return Emoji.Kiss;
        else if (text == EmojiStrings.BOUQUET) return Emoji.Bouquet;
        else if (text == EmojiStrings.COUPLE_WITH_HEART) return Emoji.CoupleWithHeart;
        else if (text == EmojiStrings.WEDDING) return Emoji.Wedding;
        else if (text == EmojiStrings.BEATING_HEART) return Emoji.BeatingHeart;
        else if (text == EmojiStrings.BROKEN_HEART) return Emoji.BrokenHeart;
        else if (text == EmojiStrings.TWO_HEARTS) return Emoji.TwoHearts;
        else if (text == EmojiStrings.SPARKLING_HEART) return Emoji.SparklingHeart;
        else if (text == EmojiStrings.GROWING_HEART) return Emoji.GrowingHeart;
        else if (text == EmojiStrings.HEART_WITH_ARROW) return Emoji.HeartWithArrow;
        else if (text == EmojiStrings.BLUE_HEART) return Emoji.BlueHeart;
        else if (text == EmojiStrings.GREEN_HEART) return Emoji.GreenHeart;
        else if (text == EmojiStrings.YELLOW_HEART) return Emoji.YellowHeart;
        else if (text == EmojiStrings.PURPLE_HEART) return Emoji.PurpleHeart;
        else if (text == EmojiStrings.HEART_WITH_RIBBON) return Emoji.HeartWithRibbon;
        else if (text == EmojiStrings.REVOLVING_HEARTS) return Emoji.RevolvingHearts;
        else if (text == EmojiStrings.HEART_DECORATION) return Emoji.HeartDecoration;
        else if (text == EmojiStrings.DIAMOND_SHAPE_WITH_A_DOT_INSIDE) return Emoji.DiamondShapeWithADotInside;
        else if (text == EmojiStrings.ELECTRIC_LIGHT_BULB) return Emoji.ElectricLightBulb;
        else if (text == EmojiStrings.ANGER_SYMBOL) return Emoji.AngerSymbol;
        else if (text == EmojiStrings.BOMB) return Emoji.Bomb;
        else if (text == EmojiStrings.SLEEPING_SYMBOL) return Emoji.SleepingSymbol;
        else if (text == EmojiStrings.COLLISION_SYMBOL) return Emoji.CollisionSymbol;
        else if (text == EmojiStrings.SPLASHING_SWEAT_SYMBOL) return Emoji.SplashingSweatSymbol;
        else if (text == EmojiStrings.DROPLET) return Emoji.Droplet;
        else if (text == EmojiStrings.DASH_SYMBOL) return Emoji.DashSymbol;
        else if (text == EmojiStrings.PILE_OF_POO) return Emoji.PileOfPoo;
        else if (text == EmojiStrings.FLEXED_BICEPS) return Emoji.FlexedBiceps;
        else if (text == EmojiStrings.DIZZY_SYMBOL) return Emoji.DizzySymbol;
        else if (text == EmojiStrings.SPEECH_BALLOON) return Emoji.SpeechBalloon;
        else if (text == EmojiStrings.WHITE_FLOWER) return Emoji.WhiteFlower;
        else if (text == EmojiStrings.HUNDRED_POINTS_SYMBOL) return Emoji.HundredPointsSymbol;
        else if (text == EmojiStrings.MONEY_BAG) return Emoji.MoneyBag;
        else if (text == EmojiStrings.CURRENCY_EXCHANGE) return Emoji.CurrencyExchange;
        else if (text == EmojiStrings.HEAVY_DOLLAR_SIGN) return Emoji.HeavyDollarSign;
        else if (text == EmojiStrings.CREDIT_CARD) return Emoji.CreditCard;
        else if (text == EmojiStrings.BANKNOTE_WITH_YEN_SIGN) return Emoji.BanknoteWithYenSign;
        else if (text == EmojiStrings.BANKNOTE_WITH_DOLLAR_SIGN) return Emoji.BanknoteWithDollarSign;
        else if (text == EmojiStrings.MONEY_WITH_WINGS) return Emoji.MoneyWithWings;
        else if (text == EmojiStrings.CHART_WITH_UPWARDS_TREND_AND_YEN_SIGN) return Emoji.ChartWithUpwardsTrendAndYenSign;
        else if (text == EmojiStrings.SEAT) return Emoji.Seat;
        else if (text == EmojiStrings.PERSONAL_COMPUTER) return Emoji.PersonalComputer;
        else if (text == EmojiStrings.BRIEFCASE) return Emoji.Briefcase;
        else if (text == EmojiStrings.MINIDISC) return Emoji.Minidisc;
        else if (text == EmojiStrings.FLOPPY_DISK) return Emoji.FloppyDisk;
        else if (text == EmojiStrings.OPTICAL_DISC) return Emoji.OpticalDisc;
        else if (text == EmojiStrings.DVD) return Emoji.Dvd;
        else if (text == EmojiStrings.FILE_FOLDER) return Emoji.FileFolder;
        else if (text == EmojiStrings.OPEN_FILE_FOLDER) return Emoji.OpenFileFolder;
        else if (text == EmojiStrings.PAGE_WITH_CURL) return Emoji.PageWithCurl;
        else if (text == EmojiStrings.PAGE_FACING_UP) return Emoji.PageFacingUp;
        else if (text == EmojiStrings.CALENDAR) return Emoji.Calendar;
        else if (text == EmojiStrings.TEAR_OFF_CALENDAR) return Emoji.TearOffCalendar;
        else if (text == EmojiStrings.CARD_INDEX) return Emoji.CardIndex;
        else if (text == EmojiStrings.CHART_WITH_UPWARDS_TREND) return Emoji.ChartWithUpwardsTrend;
        else if (text == EmojiStrings.CHART_WITH_DOWNWARDS_TREND) return Emoji.ChartWithDownwardsTrend;
        else if (text == EmojiStrings.BAR_CHART) return Emoji.BarChart;
        else if (text == EmojiStrings.CLIPBOARD) return Emoji.Clipboard;
        else if (text == EmojiStrings.PUSHPIN) return Emoji.Pushpin;
        else if (text == EmojiStrings.ROUND_PUSHPIN) return Emoji.RoundPushpin;
        else if (text == EmojiStrings.PAPERCLIP) return Emoji.Paperclip;
        else if (text == EmojiStrings.STRAIGHT_RULER) return Emoji.StraightRuler;
        else if (text == EmojiStrings.TRIANGULAR_RULER) return Emoji.TriangularRuler;
        else if (text == EmojiStrings.BOOKMARK_TABS) return Emoji.BookmarkTabs;
        else if (text == EmojiStrings.LEDGER) return Emoji.Ledger;
        else if (text == EmojiStrings.NOTEBOOK) return Emoji.Notebook;
        else if (text == EmojiStrings.NOTEBOOK_WITH_DECORATIVE_COVER) return Emoji.NotebookWithDecorativeCover;
        else if (text == EmojiStrings.CLOSED_BOOK) return Emoji.ClosedBook;
        else if (text == EmojiStrings.OPEN_BOOK) return Emoji.OpenBook;
        else if (text == EmojiStrings.GREEN_BOOK) return Emoji.GreenBook;
        else if (text == EmojiStrings.BLUE_BOOK) return Emoji.BlueBook;
        else if (text == EmojiStrings.ORANGE_BOOK) return Emoji.OrangeBook;
        else if (text == EmojiStrings.BOOKS) return Emoji.Books;
        else if (text == EmojiStrings.NAME_BADGE) return Emoji.NameBadge;
        else if (text == EmojiStrings.SCROLL) return Emoji.Scroll;
        else if (text == EmojiStrings.MEMO) return Emoji.Memo;
        else if (text == EmojiStrings.TELEPHONE_RECEIVER) return Emoji.TelephoneReceiver;
        else if (text == EmojiStrings.PAGER) return Emoji.Pager;
        else if (text == EmojiStrings.FAX_MACHINE) return Emoji.FaxMachine;
        else if (text == EmojiStrings.SATELLITE_ANTENNA) return Emoji.SatelliteAntenna;
        else if (text == EmojiStrings.PUBLIC_ADDRESS_LOUDSPEAKER) return Emoji.PublicAddressLoudspeaker;
        else if (text == EmojiStrings.CHEERING_MEGAPHONE) return Emoji.CheeringMegaphone;
        else if (text == EmojiStrings.OUTBOX_TRAY) return Emoji.OutboxTray;
        else if (text == EmojiStrings.INBOX_TRAY) return Emoji.InboxTray;
        else if (text == EmojiStrings.PACKAGE) return Emoji.Package;
        else if (text == EmojiStrings.E_MAIL_SYMBOL) return Emoji.EMailSymbol;
        else if (text == EmojiStrings.INCOMING_ENVELOPE) return Emoji.IncomingEnvelope;
        else if (text == EmojiStrings.ENVELOPE_WITH_DOWNWARDS_ARROW_ABOVE) return Emoji.EnvelopeWithDownwardsArrowAbove;
        else if (text == EmojiStrings.CLOSED_MAILBOX_WITH_LOWERED_FLAG) return Emoji.ClosedMailboxWithLoweredFlag;
        else if (text == EmojiStrings.CLOSED_MAILBOX_WITH_RAISED_FLAG) return Emoji.ClosedMailboxWithRaisedFlag;
        else if (text == EmojiStrings.POSTBOX) return Emoji.Postbox;
        else if (text == EmojiStrings.NEWSPAPER) return Emoji.Newspaper;
        else if (text == EmojiStrings.MOBILE_PHONE) return Emoji.MobilePhone;
        else if (text == EmojiStrings.MOBILE_PHONE_WITH_RIGHTWARDS_ARROW_AT_LEFT) return Emoji.MobilePhoneWithRightwardsArrowAtLeft;
        else if (text == EmojiStrings.VIBRATION_MODE) return Emoji.VibrationMode;
        else if (text == EmojiStrings.MOBILE_PHONE_OFF) return Emoji.MobilePhoneOff;
        else if (text == EmojiStrings.ANTENNA_WITH_BARS) return Emoji.AntennaWithBars;
        else if (text == EmojiStrings.CAMERA) return Emoji.Camera;
        else if (text == EmojiStrings.VIDEO_CAMERA) return Emoji.VideoCamera;
        else if (text == EmojiStrings.TELEVISION) return Emoji.Television;
        else if (text == EmojiStrings.RADIO) return Emoji.Radio;
        else if (text == EmojiStrings.VIDEOCASSETTE) return Emoji.Videocassette;
        else if (text == EmojiStrings.CLOCKWISE_DOWNWARDS_AND_UPWARDS_OPEN_CIRCLE_ARROWS) return Emoji.ClockwiseDownwardsAndUpwardsOpenCircleArrows;
        else if (text == EmojiStrings.SPEAKER_WITH_THREE_SOUND_WAVES) return Emoji.SpeakerWithThreeSoundWaves;
        else if (text == EmojiStrings.BATTERY) return Emoji.Battery;
        else if (text == EmojiStrings.ELECTRIC_PLUG) return Emoji.ElectricPlug;
        else if (text == EmojiStrings.LEFT_POINTING_MAGNIFYING_GLASS) return Emoji.LeftPointingMagnifyingGlass;
        else if (text == EmojiStrings.RIGHT_POINTING_MAGNIFYING_GLASS) return Emoji.RightPointingMagnifyingGlass;
        else if (text == EmojiStrings.LOCK_WITH_INK_PEN) return Emoji.LockWithInkPen;
        else if (text == EmojiStrings.CLOSED_LOCK_WITH_KEY) return Emoji.ClosedLockWithKey;
        else if (text == EmojiStrings.KEY) return Emoji.Key;
        else if (text == EmojiStrings.LOCK) return Emoji.Lock;
        else if (text == EmojiStrings.OPEN_LOCK) return Emoji.OpenLock;
        else if (text == EmojiStrings.BELL) return Emoji.Bell;
        else if (text == EmojiStrings.BOOKMARK) return Emoji.Bookmark;
        else if (text == EmojiStrings.LINK_SYMBOL) return Emoji.LinkSymbol;
        else if (text == EmojiStrings.RADIO_BUTTON) return Emoji.RadioButton;
        else if (text == EmojiStrings.BACK_WITH_LEFTWARDS_ARROW_ABOVE) return Emoji.BackWithLeftwardsArrowAbove;
        else if (text == EmojiStrings.END_WITH_LEFTWARDS_ARROW_ABOVE) return Emoji.EndWithLeftwardsArrowAbove;
        else if (text == EmojiStrings.ON_WITH_EXCLAMATION_MARK_WITH_LEFT_RIGHT_ARROW_ABOVE) return Emoji.OnWithExclamationMarkWithLeftRightArrowAbove;
        else if (text == EmojiStrings.SOON_WITH_RIGHTWARDS_ARROW_ABOVE) return Emoji.SoonWithRightwardsArrowAbove;
        else if (text == EmojiStrings.TOP_WITH_UPWARDS_ARROW_ABOVE) return Emoji.TopWithUpwardsArrowAbove;
        else if (text == EmojiStrings.NO_ONE_UNDER_EIGHTEEN_SYMBOL) return Emoji.NoOneUnderEighteenSymbol;
        else if (text == EmojiStrings.KEYCAP_TEN) return Emoji.KeycapTen;
        else if (text == EmojiStrings.INPUT_SYMBOL_FOR_LATIN_CAPITAL_LETTERS) return Emoji.InputSymbolForLatinCapitalLetters;
        else if (text == EmojiStrings.INPUT_SYMBOL_FOR_LATIN_SMALL_LETTERS) return Emoji.InputSymbolForLatinSmallLetters;
        else if (text == EmojiStrings.INPUT_SYMBOL_FOR_NUMBERS) return Emoji.InputSymbolForNumbers;
        else if (text == EmojiStrings.INPUT_SYMBOL_FOR_SYMBOLS) return Emoji.InputSymbolForSymbols;
        else if (text == EmojiStrings.INPUT_SYMBOL_FOR_LATIN_LETTERS) return Emoji.InputSymbolForLatinLetters;
        else if (text == EmojiStrings.FIRE) return Emoji.Fire;
        else if (text == EmojiStrings.ELECTRIC_TORCH) return Emoji.ElectricTorch;
        else if (text == EmojiStrings.WRENCH) return Emoji.Wrench;
        else if (text == EmojiStrings.HAMMER) return Emoji.Hammer;
        else if (text == EmojiStrings.NUT_AND_BOLT) return Emoji.NutAndBolt;
        else if (text == EmojiStrings.HOCHO) return Emoji.Hocho;
        else if (text == EmojiStrings.PISTOL) return Emoji.Pistol;
        else if (text == EmojiStrings.CRYSTAL_BALL) return Emoji.CrystalBall;
        else if (text == EmojiStrings.SIX_POINTED_STAR_WITH_MIDDLE_DOT) return Emoji.SixPointedStarWithMiddleDot;
        else if (text == EmojiStrings.JAPANESE_SYMBOL_FOR_BEGINNER) return Emoji.JapaneseSymbolForBeginner;
        else if (text == EmojiStrings.TRIDENT_EMBLEM) return Emoji.TridentEmblem;
        else if (text == EmojiStrings.BLACK_SQUARE_BUTTON) return Emoji.BlackSquareButton;
        else if (text == EmojiStrings.WHITE_SQUARE_BUTTON) return Emoji.WhiteSquareButton;
        else if (text == EmojiStrings.LARGE_RED_CIRCLE) return Emoji.LargeRedCircle;
        else if (text == EmojiStrings.LARGE_BLUE_CIRCLE) return Emoji.LargeBlueCircle;
        else if (text == EmojiStrings.LARGE_ORANGE_DIAMOND) return Emoji.LargeOrangeDiamond;
        else if (text == EmojiStrings.LARGE_BLUE_DIAMOND) return Emoji.LargeBlueDiamond;
        else if (text == EmojiStrings.SMALL_ORANGE_DIAMOND) return Emoji.SmallOrangeDiamond;
        else if (text == EmojiStrings.SMALL_BLUE_DIAMOND) return Emoji.SmallBlueDiamond;
        else if (text == EmojiStrings.UP_POINTING_RED_TRIANGLE) return Emoji.UpPointingRedTriangle;
        else if (text == EmojiStrings.DOWN_POINTING_RED_TRIANGLE) return Emoji.DownPointingRedTriangle;
        else if (text == EmojiStrings.UP_POINTING_SMALL_RED_TRIANGLE) return Emoji.UpPointingSmallRedTriangle;
        else if (text == EmojiStrings.DOWN_POINTING_SMALL_RED_TRIANGLE) return Emoji.DownPointingSmallRedTriangle;
        else if (text == EmojiStrings.CLOCK_FACE_ONE_OCLOCK) return Emoji.ClockFaceOneOclock;
        else if (text == EmojiStrings.CLOCK_FACE_TWO_OCLOCK) return Emoji.ClockFaceTwoOclock;
        else if (text == EmojiStrings.CLOCK_FACE_THREE_OCLOCK) return Emoji.ClockFaceThreeOclock;
        else if (text == EmojiStrings.CLOCK_FACE_FOUR_OCLOCK) return Emoji.ClockFaceFourOclock;
        else if (text == EmojiStrings.CLOCK_FACE_FIVE_OCLOCK) return Emoji.ClockFaceFiveOclock;
        else if (text == EmojiStrings.CLOCK_FACE_SIX_OCLOCK) return Emoji.ClockFaceSixOclock;
        else if (text == EmojiStrings.CLOCK_FACE_SEVEN_OCLOCK) return Emoji.ClockFaceSevenOclock;
        else if (text == EmojiStrings.CLOCK_FACE_EIGHT_OCLOCK) return Emoji.ClockFaceEightOclock;
        else if (text == EmojiStrings.CLOCK_FACE_NINE_OCLOCK) return Emoji.ClockFaceNineOclock;
        else if (text == EmojiStrings.CLOCK_FACE_TEN_OCLOCK) return Emoji.ClockFaceTenOclock;
        else if (text == EmojiStrings.CLOCK_FACE_ELEVEN_OCLOCK) return Emoji.ClockFaceElevenOclock;
        else if (text == EmojiStrings.CLOCK_FACE_TWELVE_OCLOCK) return Emoji.ClockFaceTwelveOclock;
        else if (text == EmojiStrings.MOUNT_FUJI) return Emoji.MountFuji;
        else if (text == EmojiStrings.TOKYO_TOWER) return Emoji.TokyoTower;
        else if (text == EmojiStrings.STATUE_OF_LIBERTY) return Emoji.StatueOfLiberty;
        else if (text == EmojiStrings.SILHOUETTE_OF_JAPAN) return Emoji.SilhouetteOfJapan;
        else if (text == EmojiStrings.MOYAI) return Emoji.Moyai;
        else if (text == EmojiStrings.GRINNING_FACE) return Emoji.GrinningFace;
        else if (text == EmojiStrings.SMILING_FACE_WITH_HALO) return Emoji.SmilingFaceWithHalo;
        else if (text == EmojiStrings.SMILING_FACE_WITH_HORNS) return Emoji.SmilingFaceWithHorns;
        else if (text == EmojiStrings.SMILING_FACE_WITH_SUNGLASSES) return Emoji.SmilingFaceWithSunglasses;
        else if (text == EmojiStrings.NEUTRAL_FACE) return Emoji.NeutralFace;
        else if (text == EmojiStrings.EXPRESSIONLESS_FACE) return Emoji.ExpressionlessFace;
        else if (text == EmojiStrings.CONFUSED_FACE) return Emoji.ConfusedFace;
        else if (text == EmojiStrings.KISSING_FACE) return Emoji.KissingFace;
        else if (text == EmojiStrings.KISSING_FACE_WITH_SMILING_EYES) return Emoji.KissingFaceWithSmilingEyes;
        else if (text == EmojiStrings.FACE_WITH_STUCK_OUT_TONGUE) return Emoji.FaceWithStuckOutTongue;
        else if (text == EmojiStrings.WORRIED_FACE) return Emoji.WorriedFace;
        else if (text == EmojiStrings.FROWNING_FACE_WITH_OPEN_MOUTH) return Emoji.FrowningFaceWithOpenMouth;
        else if (text == EmojiStrings.ANGUISHED_FACE) return Emoji.AnguishedFace;
        else if (text == EmojiStrings.GRIMACING_FACE) return Emoji.GrimacingFace;
        else if (text == EmojiStrings.FACE_WITH_OPEN_MOUTH) return Emoji.FaceWithOpenMouth;
        else if (text == EmojiStrings.HUSHED_FACE) return Emoji.HushedFace;
        else if (text == EmojiStrings.SLEEPING_FACE) return Emoji.SleepingFace;
        else if (text == EmojiStrings.FACE_WITHOUT_MOUTH) return Emoji.FaceWithoutMouth;
        else if (text == EmojiStrings.HELICOPTER) return Emoji.Helicopter;
        else if (text == EmojiStrings.STEAM_LOCOMOTIVE) return Emoji.SteamLocomotive;
        else if (text == EmojiStrings.TRAIN) return Emoji.Train;
        else if (text == EmojiStrings.LIGHT_RAIL) return Emoji.LightRail;
        else if (text == EmojiStrings.TRAM) return Emoji.Tram;
        else if (text == EmojiStrings.ONCOMING_BUS) return Emoji.OncomingBus;
        else if (text == EmojiStrings.TROLLEYBUS) return Emoji.Trolleybus;
        else if (text == EmojiStrings.MINIBUS) return Emoji.Minibus;
        else if (text == EmojiStrings.ONCOMING_POLICE_CAR) return Emoji.OncomingPoliceCar;
        else if (text == EmojiStrings.ONCOMING_TAXI) return Emoji.OncomingTaxi;
        else if (text == EmojiStrings.ONCOMING_AUTOMOBILE) return Emoji.OncomingAutomobile;
        else if (text == EmojiStrings.ARTICULATED_LORRY) return Emoji.ArticulatedLorry;
        else if (text == EmojiStrings.TRACTOR) return Emoji.Tractor;
        else if (text == EmojiStrings.MONORAIL) return Emoji.Monorail;
        else if (text == EmojiStrings.MOUNTAIN_RAILWAY) return Emoji.MountainRailway;
        else if (text == EmojiStrings.SUSPENSION_RAILWAY) return Emoji.SuspensionRailway;
        else if (text == EmojiStrings.MOUNTAIN_CABLEWAY) return Emoji.MountainCableway;
        else if (text == EmojiStrings.AERIAL_TRAMWAY) return Emoji.AerialTramway;
        else if (text == EmojiStrings.ROWBOAT) return Emoji.Rowboat;
        else if (text == EmojiStrings.VERTICAL_TRAFFIC_LIGHT) return Emoji.VerticalTrafficLight;
        else if (text == EmojiStrings.PUT_LITTER_IN_ITS_PLACE_SYMBOL) return Emoji.PutLitterInItsPlaceSymbol;
        else if (text == EmojiStrings.DO_NOT_LITTER_SYMBOL) return Emoji.DoNotLitterSymbol;
        else if (text == EmojiStrings.POTABLE_WATER_SYMBOL) return Emoji.PotableWaterSymbol;
        else if (text == EmojiStrings.NON_POTABLE_WATER_SYMBOL) return Emoji.NonPotableWaterSymbol;
        else if (text == EmojiStrings.NO_BICYCLES) return Emoji.NoBicycles;
        else if (text == EmojiStrings.BICYCLIST) return Emoji.Bicyclist;
        else if (text == EmojiStrings.MOUNTAIN_BICYCLIST) return Emoji.MountainBicyclist;
        else if (text == EmojiStrings.NO_PEDESTRIANS) return Emoji.NoPedestrians;
        else if (text == EmojiStrings.CHILDREN_CROSSING) return Emoji.ChildrenCrossing;
        else if (text == EmojiStrings.SHOWER) return Emoji.Shower;
        else if (text == EmojiStrings.BATHTUB) return Emoji.Bathtub;
        else if (text == EmojiStrings.PASSPORT_CONTROL) return Emoji.PassportControl;
        else if (text == EmojiStrings.CUSTOMS) return Emoji.Customs;
        else if (text == EmojiStrings.BAGGAGE_CLAIM) return Emoji.BaggageClaim;
        else if (text == EmojiStrings.LEFT_LUGGAGE) return Emoji.LeftLuggage;
        else if (text == EmojiStrings.EARTH_GLOBE_EUROPE_AFRICA) return Emoji.EarthGlobeEuropeAfrica;
        else if (text == EmojiStrings.EARTH_GLOBE_AMERICAS) return Emoji.EarthGlobeAmericas;
        else if (text == EmojiStrings.GLOBE_WITH_MERIDIANS) return Emoji.GlobeWithMeridians;
        else if (text == EmojiStrings.WAXING_CRESCENT_MOON_SYMBOL) return Emoji.WaxingCrescentMoonSymbol;
        else if (text == EmojiStrings.WANING_GIBBOUS_MOON_SYMBOL) return Emoji.WaningGibbousMoonSymbol;
        else if (text == EmojiStrings.LAST_QUARTER_MOON_SYMBOL) return Emoji.LastQuarterMoonSymbol;
        else if (text == EmojiStrings.WANING_CRESCENT_MOON_SYMBOL) return Emoji.WaningCrescentMoonSymbol;
        else if (text == EmojiStrings.NEW_MOON_WITH_FACE) return Emoji.NewMoonWithFace;
        else if (text == EmojiStrings.LAST_QUARTER_MOON_WITH_FACE) return Emoji.LastQuarterMoonWithFace;
        else if (text == EmojiStrings.FULL_MOON_WITH_FACE) return Emoji.FullMoonWithFace;
        else if (text == EmojiStrings.SUN_WITH_FACE) return Emoji.SunWithFace;
        else if (text == EmojiStrings.EVERGREEN_TREE) return Emoji.EvergreenTree;
        else if (text == EmojiStrings.DECIDUOUS_TREE) return Emoji.DeciduousTree;
        else if (text == EmojiStrings.LEMON) return Emoji.Lemon;
        else if (text == EmojiStrings.PEAR) return Emoji.Pear;
        else if (text == EmojiStrings.BABY_BOTTLE) return Emoji.BabyBottle;
        else if (text == EmojiStrings.HORSE_RACING) return Emoji.HorseRacing;
        else if (text == EmojiStrings.RUGBY_FOOTBALL) return Emoji.RugbyFootball;
        else if (text == EmojiStrings.EUROPEAN_POST_OFFICE) return Emoji.EuropeanPostOffice;
        else if (text == EmojiStrings.RAT) return Emoji.Rat;
        else if (text == EmojiStrings.MOUSE) return Emoji.Mouse;
        else if (text == EmojiStrings.OX) return Emoji.Ox;
        else if (text == EmojiStrings.WATER_BUFFALO) return Emoji.WaterBuffalo;
        else if (text == EmojiStrings.COW) return Emoji.Cow;
        else if (text == EmojiStrings.TIGER) return Emoji.Tiger;
        else if (text == EmojiStrings.LEOPARD) return Emoji.Leopard;
        else if (text == EmojiStrings.RABBIT) return Emoji.Rabbit;
        else if (text == EmojiStrings.CAT) return Emoji.Cat;
        else if (text == EmojiStrings.DRAGON) return Emoji.Dragon;
        else if (text == EmojiStrings.CROCODILE) return Emoji.Crocodile;
        else if (text == EmojiStrings.WHALE) return Emoji.Whale;
        else if (text == EmojiStrings.RAM) return Emoji.Ram;
        else if (text == EmojiStrings.GOAT) return Emoji.Goat;
        else if (text == EmojiStrings.ROOSTER) return Emoji.Rooster;
        else if (text == EmojiStrings.DOG) return Emoji.Dog;
        else if (text == EmojiStrings.PIG) return Emoji.Pig;
        else if (text == EmojiStrings.DROMEDARY_CAMEL) return Emoji.DromedaryCamel;
        else if (text == EmojiStrings.BUSTS_IN_SILHOUETTE) return Emoji.BustsInSilhouette;
        else if (text == EmojiStrings.TWO_MEN_HOLDING_HANDS) return Emoji.TwoMenHoldingHands;
        else if (text == EmojiStrings.TWO_WOMEN_HOLDING_HANDS) return Emoji.TwoWomenHoldingHands;
        else if (text == EmojiStrings.THOUGHT_BALLOON) return Emoji.ThoughtBalloon;
        else if (text == EmojiStrings.BANKNOTE_WITH_EURO_SIGN) return Emoji.BanknoteWithEuroSign;
        else if (text == EmojiStrings.BANKNOTE_WITH_POUND_SIGN) return Emoji.BanknoteWithPoundSign;
        else if (text == EmojiStrings.OPEN_MAILBOX_WITH_RAISED_FLAG) return Emoji.OpenMailboxWithRaisedFlag;
        else if (text == EmojiStrings.OPEN_MAILBOX_WITH_LOWERED_FLAG) return Emoji.OpenMailboxWithLoweredFlag;
        else if (text == EmojiStrings.POSTAL_HORN) return Emoji.PostalHorn;
        else if (text == EmojiStrings.NO_MOBILE_PHONES) return Emoji.NoMobilePhones;
        else if (text == EmojiStrings.TWISTED_RIGHTWARDS_ARROWS) return Emoji.TwistedRightwardsArrows;
        else if (text == EmojiStrings.CLOCKWISE_RIGHTWARDS_AND_LEFTWARDS_OPEN_CIRCLE_ARROWS) return Emoji.ClockwiseRightwardsAndLeftwardsOpenCircleArrows;
        else if (text == EmojiStrings.CLOCKWISE_RIGHTWARDS_AND_LEFTWARDS_OPEN_CIRCLE_ARROWS_WITH_CIRCLED_ONE_OVERLAY) return Emoji.ClockwiseRightwardsAndLeftwardsOpenCircleArrowsWithCircledOneOverlay;
        else if (text == EmojiStrings.ANTICLOCKWISE_DOWNWARDS_AND_UPWARDS_OPEN_CIRCLE_ARROWS) return Emoji.AnticlockwiseDownwardsAndUpwardsOpenCircleArrows;
        else if (text == EmojiStrings.LOW_BRIGHTNESS_SYMBOL) return Emoji.LowBrightnessSymbol;
        else if (text == EmojiStrings.HIGH_BRIGHTNESS_SYMBOL) return Emoji.HighBrightnessSymbol;
        else if (text == EmojiStrings.SPEAKER_WITH_CANCELLATION_STROKE) return Emoji.SpeakerWithCancellationStroke;
        else if (text == EmojiStrings.SPEAKER_WITH_ONE_SOUND_WAVE) return Emoji.SpeakerWithOneSoundWave;
        else if (text == EmojiStrings.BELL_WITH_CANCELLATION_STROKE) return Emoji.BellWithCancellationStroke;
        else if (text == EmojiStrings.MICROSCOPE) return Emoji.Microscope;
        else if (text == EmojiStrings.TELESCOPE) return Emoji.Telescope;
        else if (text == EmojiStrings.CLOCK_FACE_ONE_THIRTY) return Emoji.ClockFaceOneThirty;
        else if (text == EmojiStrings.CLOCK_FACE_TWO_THIRTY) return Emoji.ClockFaceTwoThirty;
        else if (text == EmojiStrings.CLOCK_FACE_THREE_THIRTY) return Emoji.ClockFaceThreeThirty;
        else if (text == EmojiStrings.CLOCK_FACE_FOUR_THIRTY) return Emoji.ClockFaceFourThirty;
        else if (text == EmojiStrings.CLOCK_FACE_FIVE_THIRTY) return Emoji.ClockFaceFiveThirty;
        else if (text == EmojiStrings.CLOCK_FACE_SIX_THIRTY) return Emoji.ClockFaceSixThirty;
        else if (text == EmojiStrings.CLOCK_FACE_SEVEN_THIRTY) return Emoji.ClockFaceSevenThirty;
        else if (text == EmojiStrings.CLOCK_FACE_EIGHT_THIRTY) return Emoji.ClockFaceEightThirty;
        else if (text == EmojiStrings.CLOCK_FACE_NINE_THIRTY) return Emoji.ClockFaceNineThirty;
        else if (text == EmojiStrings.CLOCK_FACE_TEN_THIRTY) return Emoji.ClockFaceTenThirty;
        else if (text == EmojiStrings.CLOCK_FACE_ELEVEN_THIRTY) return Emoji.ClockFaceElevenThirty;
        else if (text == EmojiStrings.CLOCK_FACE_TWELVE_THIRTY) return Emoji.ClockFaceTwelveThirty;
        return Emoji.None;
    }

    public static bool Matches(this StateContext ctx, ButtonLabel label) => ctx.Message is TextMessageContent text && label.Matches(text.Text);
}
